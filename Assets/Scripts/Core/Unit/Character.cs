﻿using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class Character : Unit
    {
        public string characterName;
        public int slot;
        public bool isLocal;
        public CmdManager cmdMgr { get; protected set; }
        private int input;

        public Character(string characterName, CharacterConfig config, int slot, bool isLocal) : base(config)
        {
            this.characterName = characterName;
            this.slot = slot;
            this.isLocal = isLocal;       
            cmdMgr = new CmdManager(config.commandContent, this); 
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

    }

}
