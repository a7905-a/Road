
using UnityEngine;
using System.Collections;
using ProjectRoad.Manager;
using ProjectRoad.Controller;

namespace ProjectRoad.Unit
{
    public class Unit : BaseUnit
    {
        [Header("VFX 위치 참조")]
        [SerializeField] private Transform attackPoint;
        [SerializeField] private Transform hitPoint;

        [Header("시각 효과 데이터")]
        [SerializeField] private UnitVisualData visualData;

        // 캐싱 컴포넌트
        private Move move;
        private AttackController attackController;

        protected override void Awake()
        {
            base.Awake(); 
            move = GetComponent<Move>();
            attackController = GetComponent<AttackController>();
        }

        protected override void Start()
        {
            base.Start();
            UnitSelectionManager.Instance.AddUnit(gameObject);
        }

        private void Update()
        {
            if (move.isCommandedMove)
            {
                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }

        }

        protected override void Retire()
        {
            UnitSelectionManager.Instance.RemoveUnit(gameObject);
            base.Retire();
        }

        protected override void OnHit()
        {
            if (visualData != null && visualData.hitEffect != null && hitPoint != null)
            {
                Instantiate(visualData.hitEffect, hitPoint.position, hitPoint.rotation);
            }
        }

        //애니메이션 이벤트에 할당
        public void AttackEffect()
        {
            // 발사 이펙트 생성
            if (visualData.muzzleFlash != null)
            {
                //Instantiate(visualData.muzzleFlash, attackPoint.position, attackPoint.rotation);
                GameObject muzzleFlash = PoolManager.instance.ActiveObject(0); // 0번 인덱스가 총구 이펙트라고 가정
                PoolManager.instance.SetPosition(muzzleFlash, attackPoint.position);
                
            }

            // attackController를 사용하여 타겟 가져오기
            Transform currentTarget = attackController.targetToAttack;
            
            if (currentTarget != null)
            {
                //GameObject lineEffect = Instantiate(visualData.bulletLine, attackPoint.position, attackPoint.rotation);
                GameObject lineEffect = PoolManager.instance.ActiveObject(2);
                PoolManager.instance.SetPosition(lineEffect, attackPoint.position);
                
                LineRenderer lr = lineEffect.GetComponent<LineRenderer>();

                Vector3 startPos = attackPoint.position;
                Vector3 endPos = currentTarget.position + Vector3.up * 1.0f;

                StartCoroutine(DrawLineTrail(lr, startPos, endPos));
            }
        }
        IEnumerator DrawLineTrail(LineRenderer lr, Vector3 startPos, Vector3 endPos)
        {
            // 1. 선의 시작점(0)과 도착점(1)을 설정
            lr.SetPosition(0, startPos); // 총구
            lr.SetPosition(1, endPos); // 적 유닛 몸통

            // 0.05초 동안 투명해지는 페이드아웃 처리
            Color baseColor = lr.startColor; // 매테리얼의 기본 색상을 가져옵니다 (Alpha는 1로 가정)
            lr.startColor = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
            lr.endColor = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
            
            float duration = 0.2f;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                
                // 투명도 조절: time/duration이 0->1로 갈 때, Alpha는 1->0으로 투명
                float alpha = Mathf.Lerp(1f, 0f, time / duration);
                
                // 선의 시작 색상과 끝 색상의 투명도를 업데이트
                lr.startColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lr.endColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

                yield return null; // 다음 프레임까지 대기
            }

            // 페이드아웃이 끝나면 오브젝트를 지우기
            PoolManager.instance.DeactiveObject(lr.gameObject);
        }

    }
}
