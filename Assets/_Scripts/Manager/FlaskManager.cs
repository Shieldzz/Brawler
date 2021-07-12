using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class FlaskManager : MonoBehaviour
    {
        static private bool m_created = false;

        static private FlaskManager m_instance = null;
        static public FlaskManager Instance
        {
            get
            {
                if (!m_instance)
                    m_instance = new FlaskManager();
                return m_instance;
            }
        }
        
        public FlaskData m_speedFlaskData;
        public FlaskData m_dashFlaskData;
        public FlaskData m_jumpFlaskData;
        public FlaskData m_reviveFlaskData;
        public FlaskData m_chainGaugeDecayFlaskData;
        public FlaskData m_chainTimerFlaskData;
        public FlaskData m_superArmorFlaskData;
        public FlaskData m_adrenalineFlaskData;
        public FlaskData m_hardcoreFlaskData;

        public Flasks[] m_meleeCharacterFlasks = null;
        public Flasks[] m_distanceCharacterFlasks = null;

        public FlaskData[] m_meleeEquipedFlaskData = null;
        public FlaskData[] m_distanceEquipedFlaskData = null;

        public List<FlaskData> m_ownFlask = null;

        private void Awake()
        {
            if (FindObjectsOfType<FlaskManager>().Length > 1)
            {
                Destroy(this);
                return;
            }

            m_instance = this;
            if (!m_created)
                DontDestroyOnLoad(this);

        }

        void Start()
        {
            LoadAllFlask();

            m_meleeCharacterFlasks = new Flasks[3];
            m_distanceCharacterFlasks = new Flasks[3];

            m_meleeEquipedFlaskData = new FlaskData[3];
            m_distanceEquipedFlaskData = new FlaskData[3];

            m_ownFlask = new List<FlaskData>();
        }

        public void RemoveEquipedFlask(int flaskIndex, CharacterEnum character)
        {
            if (flaskIndex >= 3)
                return;
            if (flaskIndex < 0)
                return;

            FlaskData[] flaskDataList = GetFlaskDataFromPlayerEnum(character);
            flaskDataList[flaskIndex] = null;

            Flasks[] flasks = GetCharacterFlaskList(character);
            flasks[flaskIndex] = null;
        }

        public void AddFlaskDataToPlayer(CharacterEnum character, FlaskData data, int flaskIdx)
        {
            if (!IsFlaskIsOwn(data))
                return;
            if (flaskIdx < 0)
                return;

            FlaskData[] flaskDataList = GetFlaskDataFromPlayerEnum(character);
            flaskDataList[flaskIdx] = data;

            AddTransformedDataFlaskToCharacterFlaskList(character, data, flaskIdx);
        }

        private void AddTransformedDataFlaskToCharacterFlaskList(CharacterEnum character, FlaskData data, int flaskIdx)
        {
            Flasks flask = TransformFlaskDataToFlask(data);
            Flasks[] flaskList = GetCharacterFlaskList(character);

            flaskList[flaskIdx] = flask;
        }

        private Flasks TransformFlaskDataToFlask(FlaskData data)
        {
            switch (data.m_currentFlaskType)
            {
                case FlaskData.FLASKTYPE.DASH:
                    return GetComponent<DashFlask>();
                case FlaskData.FLASKTYPE.JUMP:
                    return GetComponent<JumpHeightBoostDrug>();
                case FlaskData.FLASKTYPE.SPEED:
                    return GetComponent<SpeedBoostDrug>();
                case FlaskData.FLASKTYPE.REVIVE:
                    return GetComponent<ReviveFlask>();
                case FlaskData.FLASKTYPE.CHAINGAUGEDECAY:
                    return GetComponent<ChainGaugeDecayFlask>();
                case FlaskData.FLASKTYPE.CHAINTIMER:
                    return GetComponent<ChainTimerFlask>();
                case FlaskData.FLASKTYPE.SUPERARMOR:
                    return GetComponent<SuperArmorFlask>();
                case FlaskData.FLASKTYPE.ADRENALINE:
                    return GetComponent<AdrenalineFlask>();
                case FlaskData.FLASKTYPE.HARDCORE:
                    return GetComponent<HardcoreFlask>();
                default:
                    return null;
            }
        }

        private Flasks[] GetCharacterFlaskList(CharacterEnum character)
        {
            if (character == CharacterEnum.MELEE)
                return m_meleeCharacterFlasks;
            else if (character == CharacterEnum.DISTANCE)
                return m_distanceCharacterFlasks;

            return null;
        }

        public void OwnNewFlask(FlaskData data)
        {
            if (IsFlaskIsOwn(data))
                return;

            m_ownFlask.Add(data);
        }

        private FlaskData[] GetFlaskDataFromPlayerEnum(CharacterEnum character)
        {
            if (character == CharacterEnum.MELEE)
                return m_meleeEquipedFlaskData;
            else if (character == CharacterEnum.DISTANCE)
                return m_distanceEquipedFlaskData;
            else
                return null;
        }

        private bool IsFlaskIsOwn(FlaskData data)
        {
            if (m_ownFlask.Contains(data))
                return true;
            return false;
        }

        void Update()
        {
        }

        private void LoadAllFlask()
        {
            if(m_speedFlaskData)
                gameObject.AddComponent<SpeedBoostDrug>().LoadFlaskFromData(m_speedFlaskData);
            if(m_dashFlaskData)
                gameObject.AddComponent<DashFlask>().LoadFlaskFromData(m_dashFlaskData);
            if(m_jumpFlaskData)
                gameObject.AddComponent<JumpHeightBoostDrug>().LoadFlaskFromData(m_jumpFlaskData);
            if (m_reviveFlaskData)
                gameObject.AddComponent<ReviveFlask>().LoadFlaskFromData(m_reviveFlaskData);

            if (m_chainGaugeDecayFlaskData)
                gameObject.AddComponent<ChainGaugeDecayFlask>().LoadFlaskFromData(m_chainGaugeDecayFlaskData);
            if (m_chainTimerFlaskData)
                gameObject.AddComponent<ChainTimerFlask>().LoadFlaskFromData(m_chainTimerFlaskData);
            if (m_superArmorFlaskData)
                gameObject.AddComponent<SuperArmorFlask>().LoadFlaskFromData(m_superArmorFlaskData);
            if (m_adrenalineFlaskData)
                gameObject.AddComponent<AdrenalineFlask>().LoadFlaskFromData(m_adrenalineFlaskData);
            if (m_hardcoreFlaskData)
                gameObject.AddComponent<HardcoreFlask>().LoadFlaskFromData(m_hardcoreFlaskData);

        }
    }

}