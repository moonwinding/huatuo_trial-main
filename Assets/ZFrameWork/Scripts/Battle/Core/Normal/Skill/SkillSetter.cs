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
        public string SkillTimeLineRes;//����չʾ���ܵ�TimeLine������Դ

        public AreaType areaType = AreaType.Sphere;//����ѡ��
        public TargetSelectType targetType = TargetSelectType.Self;//Ŀ�����Ӫѡ��
        public float range;//�˺��ķ�Χ
        public float angle;//�˺��ĽǶȷ�Χ

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

