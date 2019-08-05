using System;
using System.Collections;
using System.Collections.Generic;

namespace bluebean.UGFramework.ConfigData {
    [Serializable]
    public class ConfigDataCharacter {
        private int m_ID;
        public int ID {
            get {return m_ID;}
            set {m_ID = value;}
        }
        private string m_Name;
        public string Name {
            get {return m_Name;}
            set {m_Name = value;}
        }
        private string m_LittleHeadIcon;
        public string LittleHeadIcon {
            get {return m_LittleHeadIcon;}
            set {m_LittleHeadIcon = value;}
        }
        private string m_MediumHeadIcon;
        public string MediumHeadIcon {
            get {return m_MediumHeadIcon;}
            set {m_MediumHeadIcon = value;}
        }
        private string m_BigHeadIcon;
        public string BigHeadIcon {
            get {return m_BigHeadIcon;}
            set {m_BigHeadIcon = value;}
        }
    }
	
}