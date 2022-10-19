using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Skill
{
    [CreateAssetMenu(menuName = "Creat/NewSkillSetter")]
    public class SkillSetter : ScriptableObject
    {
        public int Id;
        public float Cost;
        public float Cd;
        public string SkillTimeLineRes;//用来展示节能的TimeLine表现资源

        public AreaType areaType = AreaType.Sphere;//区域选择
        public TargetSelectType targetType = TargetSelectType.Self;//目标的阵营选择
        public float range;//伤害的范围
        public float angle;//伤害的角度范围

        public static SkillSetter LoadDataFromFile(string Path)
        {
            var textAsset = AssetManager.Instance.LoadAsset<SkillSetter>(Path);
            return textAsset;
        }

        public Skill GetSkill(){
            var skill = new Skill() {
                Id = this.Id,
                Cost = this.Cost,
                Cd = this.Cd,
                SkillTimeLineRes = this.SkillTimeLineRes,
                areaType = this.areaType,
                targetType = this.targetType,
                range = this.range,
                angle = this.angle
            };
            return skill;
        }
    }
}

