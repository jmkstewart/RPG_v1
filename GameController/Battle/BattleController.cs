using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model;
using Model.Battle;
using View.Battle;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;

namespace GameController.Battle {
    public class BattleController : IController, MenuItemSelectListener {
        private enum BattleState { ActiveTime, StoppedTime }

        private readonly int _timeBetweenBattleTimeUpdates = 100;

        private BattleState _battleState = BattleState.ActiveTime;
        private List<BattleObject> _playerParty;
        private List<BattleObject> _enemyParty;
        private BattleObjectMenuView _battleMenuView;
        private BattleView _battleView;
        private BattleObject _activeBattleObject = null;
        private Queue<BattleObject> _activeBattleObjectsToProcess;
        private List<Keys> _processedPressedKeys;
        private int _timeSinceLastBattleTimeUpdate = 100;

        private int _id;
        private Player _player;
        private Game _game;

        public BattleController() {
            _playerParty = new List<BattleObject>();
            _enemyParty = new List<BattleObject>();
            _activeBattleObjectsToProcess = new Queue<BattleObject>();
            _processedPressedKeys = new List<Keys>();
        }

        public void Init(int id, Player player) {
            _id = id;
            _player = player;
        }

        public void LoadContent(Game game) {
            // based on the Id, pull up the xml file of the battle and load the enemies
            _game = game;

            LoadEnemies(_id);
            LoadParty(_player);
            _battleView = new BattleView(game, _playerParty, _enemyParty);
        }

        private void LoadEnemies(int id) {
            var enemy1 = _game.Content.Load<BattleObject>(@"BattleXML/Enemy1"); //new BattleObject() { HP = 10, MP = 10, Name = "bort", Id = 0, Speed = 40 };
            _enemyParty.Add(enemy1);
        }

        private void LoadParty(Player player) {
            var player1 = _game.Content.Load<BattleObject>(@"BattleXML/Character1");
            _playerParty.Add(player1);
        }

        public void ItemClicked(string item) {
            // depending on the attack allow the user to select target
            // skipping that for now

            // do actual attack
            Attack(_activeBattleObject, _enemyParty[0], item);
        }

        private void Attack(BattleObject from, BattleObject to, string actionText) {
            // find the action from the list loaded in
            var actions = _game.Content.Load<Actions>(@"BattleXML/Actions");
            if(actions.ActionList.Any(x => x.Name == actionText)) {
                var action = actions.ActionList.First(x => x.Name == actionText);

                // determine the damage
                var damage = action.Damage * from.Str;
                to.HP -= damage;

                // start damage display (text of numbers popping up)
                InteractableBattleObjectView toBattleView = null;
                if(to.Enemy) {
                    toBattleView = _battleView.EnemyPartyView.First(x => x.Name == to.Name);
                } else {
                    toBattleView = _battleView.PlayerPartyView.First(x => x.Name == to.Name);
                }
                toBattleView.StartDamageText(damage);

                // start damage animation

                _battleState = BattleState.ActiveTime;
                _activeBattleObject = null;
                _battleMenuView = null;
            }
        }

        public void Update(GameTime gameTime) {
            if(_battleState == BattleState.ActiveTime) {
                UpdateActive(gameTime);
            } else {
                UpdateStopped(gameTime);
            }

            _battleView.Update(gameTime);
        }

        private void UpdateActive(GameTime gameTime) {
            // count down one speed unit for each 100 milliseconds
            _timeSinceLastBattleTimeUpdate += gameTime.ElapsedGameTime.Milliseconds;
            if(_timeSinceLastBattleTimeUpdate > _timeBetweenBattleTimeUpdates) {
                BattleTimeUpdate();
                _timeSinceLastBattleTimeUpdate -= _timeBetweenBattleTimeUpdates;
            }
        }

        private void UpdateStopped(GameTime gameTime) {
            // figure out if there is an object ready
            if(_activeBattleObject == null && _activeBattleObjectsToProcess.Count > 0) {
                _activeBattleObject = _activeBattleObjectsToProcess.Dequeue();

                if(_activeBattleObject.Enemy) {
                    Attack(_activeBattleObject, _playerParty[0], _activeBattleObject.AttackList[0]);
                } else {
                    _battleMenuView = new BattleObjectMenuView(_game, _activeBattleObject, this);
                }
            }

            // do keyboard navigation of the menu
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach(var key in pressedKeys) {
                if(!_processedPressedKeys.Contains(key)) {
                    _processedPressedKeys.Add(key);
                    _battleMenuView.KeyPressed(key);
                }
            }

            _processedPressedKeys = pressedKeys.ToList();
        }

        public void BattleTimeUpdate() {
            _playerParty.ForEach(x => x.CurrentTimeToAction -= x.Speed);
            _enemyParty.ForEach(x => x.CurrentTimeToAction -= x.Speed);

            var playersToAction = _playerParty.Where(x => x.CurrentTimeToAction <= 0).ToList();
            var enemiesToAction = _enemyParty.Where(x => x.CurrentTimeToAction <= 0).ToList();

            playersToAction.ForEach(x => StartActionFor(x));
            enemiesToAction.ForEach(x => StartActionFor(x));

            playersToAction.ForEach(x => x.CurrentTimeToAction = BattleObject.TimeToAction);
            enemiesToAction.ForEach(x => x.CurrentTimeToAction = BattleObject.TimeToAction);
        }

        private void StartActionFor(BattleObject battleObject) {
            _battleState = BattleState.StoppedTime;
            _activeBattleObjectsToProcess.Enqueue(battleObject);
        }

        public void DrawComponents(GameTime gameTime, SpriteBatch spriteBatch) {
            _battleView.Draw(gameTime, spriteBatch, Vector2.Zero);

            if(_battleMenuView != null) {
                _battleMenuView.Draw(gameTime, spriteBatch, new Vector2(800, 400));
            }
        }
    }
}
