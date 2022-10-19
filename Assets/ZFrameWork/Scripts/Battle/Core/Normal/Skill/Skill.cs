using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Skill {

    public class Skill
    {
        public int Id;
        public float Cost;
        public float Cd;
        public string SkillTimeLineRes;//����չʾ���ܵ�TimeLine������Դ

        public AreaType areaType = AreaType.Sphere;//����ѡ��
        public TargetSelectType targetType = TargetSelectType.Self;//Ŀ�����Ӫѡ��
        public float range;//�˺��ķ�Χ
        public float angle;//�˺��ĽǶȷ�Χ
        
        private Unit bindUnit;
        private float NextReleaseTime;
        private UnitManager unitManger;
        public bool IsExistUnitInRange() {
            if (unitManger == null)
                unitManger = BattleField.Instance.GetUnitManager();
            var units = unitManger.GetUnitsSkillInfo(this, bindUnit.Id);
            return units != null && units.Count >0;
        }
        public void BindUnit(Unit pUnit) {
            bindUnit = pUnit;
        }
        
        public bool IsCanRelease() {
            var curTime = Time.realtimeSinceStartup;
            if (curTime < NextReleaseTime) {
                return false;
            }
            if (bindUnit.mp < Cost) {
                return false;
            }
            return true;
        }
        public void OnRealse() {
            NextReleaseTime = Time.realtimeSinceStartup + Cd;
            bindUnit.OnCostMp(Cost);
        }
    }
}

