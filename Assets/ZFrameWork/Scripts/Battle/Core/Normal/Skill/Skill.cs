using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Skill {

    public class Skill
    {
        public int Id;
        public float Cost;
        public float Cd;
        public string SkillTimeLineRes;//用来展示节能的TimeLine表现资源

        public AreaType areaType = AreaType.Sphere;//区域选择
        public TargetSelectType targetType = TargetSelectType.Self;//目标的阵营选择
        public float range;//伤害的范围
        public float angle;//伤害的角度范围
        
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

