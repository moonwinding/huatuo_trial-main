using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using UnityEngine.Timeline;

namespace Battle.Skill
{
    public class SkillReleaser : MonoBehaviour
    {
        private PlayableDirector playableDirector;
        private Unit unit;
        private PlayableAsset skillAsset;
        private TimeLineInfoer timeLineInfoer;
        private System.Action onFinish;
        private void Awake()
        {
            playableDirector = this.gameObject.GetComponent<PlayableDirector>();
            playableDirector.playOnAwake = false;
            timeLineInfoer = this.gameObject.GetComponent<TimeLineInfoer>();
            if (timeLineInfoer == null) {
                timeLineInfoer = this.gameObject.AddComponent<TimeLineInfoer>();
            }
        }
        public void BindUnit(Unit pUnit){unit = pUnit;}
        public bool IsCanRelease(Skill pSkill) {
            return pSkill.IsCanRelease();
        }
        public bool ReleaseSkill(Skill pSkill,GameObject pTarget,System.Action pFinish){
            if (!IsCanRelease(pSkill))
            {
                pFinish?.Invoke();
                return false;
            }
            timeLineInfoer.target = pTarget.transform;
            
            pSkill.OnRealse();
            onFinish = pFinish;
            if(skillAsset == null)
                skillAsset = AssetManager.Instance.LoadAsset<PlayableAsset>(pSkill.SkillTimeLineRes);
            playableDirector.playableAsset = skillAsset;
            SetPlayableAssetTarget(skillAsset);
            playableDirector.stopped -= OnFinish;
            playableDirector.stopped += OnFinish;
            playableDirector.Play();
            return true;
        }
        public void SetPlayableAssetTarget(PlayableAsset pAsset) {
            //foreach (var o in pAsset.outputs)
            //{
            //    //�ҵ���������� ����ͨ������Ч�Ǹ���� UnityEngine.Timeline.ControlTrack
            //    if (o.sourceObject != null && o.sourceObject.GetType() == typeof(UnityEngine.Timeline.ControlTrack))
            //    {
            //        var controlTrack = o.sourceObject as ControlTrack;
            //        //�����������е�����Ƭ��
            //        foreach (var timelineClip in controlTrack.GetClips())
            //        {
            //            ControlPlayableAsset clip = timelineClip.asset as ControlPlayableAsset;

            //            bool isvalid;
            //            //�ؼ���������ȡ��ͨ�������ҵ�ParentObject����
            //            Object obj = playableDirector.GetReferenceValue(clip.sourceGameObject.exposedName, out isvalid);

            //            //�ؼ��������޸ģ�ͨ���������õķ�ʽ����ParentObject
            //            // PropertyName propertyName = clip.sourceGameObject.exposedName;
            //            // playableDirector.SetReferenceValue(propertyName, obj);
            //        }
            //    }
            //}
        }
        public void OnFinish(PlayableDirector pPlayableDirector) {
            onFinish?.Invoke();
        }
    }
}

