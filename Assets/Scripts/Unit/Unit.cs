
using UnityEngine;
using System.Collections;


public class Unit : BaseUnit
{
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform hitPoint;
    
    protected override void Start()
    {
        base.Start();
        UnitSelectionManager.Instance.AddUnit(gameObject);
    }

    void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
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
        if (hitPoint != null)
        {
            //Instantiate(unitData.hitEffect, hitPoint.position, hitPoint.rotation);
        }
    }

    //애니메이션 이벤트에 할당
    public void AttackEffect()
    {
        Instantiate(unitData.muzzleFlash, attackPoint.position, attackPoint.rotation);
        Transform currentTarget = GetComponent<AttackController>().targetToAttack;
        if (currentTarget != null)
        {
            GameObject lineEffect = Instantiate(unitData.bulletLine, attackPoint.position, attackPoint.rotation);
            LineRenderer lr = lineEffect.GetComponent<LineRenderer>();

            Vector3 startPos = attackPoint.position;
            Vector3 endPos = currentTarget.transform.position + Vector3.up * 1.0f;

            StartCoroutine(DrawLineTrail(lr, startPos, endPos));
        }
    }
    IEnumerator DrawLineTrail(LineRenderer lr, Vector3 startPos, Vector3 endPos)
{
    // 1. 선의 시작점(0)과 도착점(1)을 설정합니다.
    // (이동 로직이 필요 없는 이유: 0.05초는 너무 짧아서 적이 이동해도 신경 쓰이지 않습니다)
    lr.SetPosition(0, startPos); // 총구
    lr.SetPosition(1, endPos); // 적 유닛 몸통

    // 2. 0.05초 동안 투명해지는 페이드아웃(Fade-Out) 처리
    // (LineRenderer의 매테리얼이 투명도 변경(Alpha)을 지원해야 정상 작동합니다)
    Color baseColor = lr.startColor; // 매테리얼의 기본 색상을 가져옵니다 (Alpha는 1로 가정)
    
    float duration = 0.2f; // 질문자님이 요청하신 0.05초
    float time = 0;

    while (time < duration)
    {
        time += Time.deltaTime;
        
        // 투명도 조절: time/duration이 0->1로 갈 때, Alpha는 1->0으로 투명해집니다.
        float alpha = Mathf.Lerp(1f, 0f, time / duration);
        
        // 선의 시작 색상과 끝 색상의 투명도를 업데이트합니다.
        lr.startColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        lr.endColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

        yield return null; // 다음 프레임까지 대기
    }

    // 3. 페이드아웃이 끝나면 오브젝트를 지웁니다.
    // (보류하신 PoolManager를 나중에 만드신다면, 여기서 Destroy 대신 풀로 반환하면 됩니다)
    Destroy(lr.gameObject);
}

}
