﻿using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class Character : Unit
    {
        public string characterName { get; private set; }
        public int slot { get; private set; }
        public bool isLocal { get; private set; }
        public CmdManager cmdMgr { get; protected set; }
        private int input;

        public Character(string characterName, CharacterConfig config, int slot, bool isLocal) : base(config)
        {
            this.characterName = characterName;
            this.slot = slot;
            this.isLocal = isLocal;       
            cmdMgr = new CmdManager(config.commandContent, this);
            moveCtr = new CharacterMoveCtrl(this);
        }

        public override void OnUpdate(Number deltaTime)
        {
            base.OnUpdate(deltaTime);
            cmdMgr.Update(input);
        }

        public void UpdateInput(int input)
        {
            this.input = input;
        }

        public Vector GetP2Dist()
        {
            var enemy = this.world.teamInfo.GetEnemy(this);
            return enemy.position - this.position;
        }

    }

}
