# Project Road

<br>

##  1. 프로젝트 소개 
- **사용 엔진:** Unity 6000.3.2f1
- **사용 언어:** C#
- **핵심 기술:**  `FSM`, `객체지향(OOP)`, `NavMesh`, `Raycast`

<br>

##  2. 시연 영상

*  **https://youtu.be/PzkhqMNmxVw?si=X8nfed5sdwqFWE5m**

<br>

##  3. 사용한 아키텍처

### A. 플레이어 유닛과 적 유닛 행동에 따른 FSM  설계
플레이어 유닛보다 복잡한 적 유닛의 애니메이션 전환으로 인해 두 가지 다른 FSM 방식을 혼용하였습니다.

| 구분 | 플레이어 유닛 | 적 유닛 |
| :--- | :--- | :--- |
| **방식** | **애니메이터 기반 Animator-Driven FSM** | **인터페이스 기반 Interface-based State Pattern** |
| **구조** | Animator Controller 활용 | C# Interface (`IState`) 활용 |
| **선택 이유** | 이동과 공격 전환에 있어 **즉각적인 애니메이션 반응**과 상태 전환이 직관적이고 시각적으로 표현되서 선택. | 추격, 정찰, 공격 등 **복잡한 판단 로직**이 필요하며, 애니메이션과 로직을 분리하여 확장성(OCP)을 확보해야 해서 선택. |
| **기대 효과** | `상태 머신 동작 StateMachineBehaviour`를 통해 각 상태의 로직을 독립적인 스크립트로 관리하여 유지보수성 상승. | 새로운 행동 패턴 추가 시 기존 코드를 수정하지 않는 유연한 구조 확보. |

<br>

### B. 데이터 무결성을 위한 방어적 프로그래밍
프로젝트 특성상 유닛의 생성과 파괴가 빈번하므로, 관리 소홀로 인한 데이터 오염을 원천 차단하기 위해 캡슐화 원칙을 준수했습니다.

#### 1. IReadOnlyList를 활용한 리스트 보호
유닛 관리 매니저(`UnitSelectionManager`)가 외부 클래스에 리스트를 제공할 때, 수정 불가능한 `IReadOnlyList<T>` 인터페이스를 반환합니다.

```csharp
// [Before] public List는 외부에서 마음대로 Add/Clear가 가능하여 위험함
// public List<GameObject> AllUnits; 

// [After] 내부는 List로 관리하되, 외부는 읽기 전용으로만 공개
[SerializeField] private List<GameObject> _allUnits = new List<GameObject>();
public IReadOnlyList<GameObject> AllUnits => _allUnits;
```

<br>

### C. Raycast와 NavMesh를 활용한 RTS 이동 시스템
3인칭 탑뷰 시점에서 플레이어의 입력 정확히 월드 좌표로 변환하고 Raycast와 NavMesh를 사용하여 이동 로직을 구현했습니다.

* **좌표 획득 (Raycast):**
  카메라에서 마우스 화면 좌표로 광선 Ray을 투사하여, Ground 레이어로 지정된 바닥과의 충돌 지점(`Vector3`)을 정밀하게 감지합니다.
* **경로 계산 및 이동 (NavMeshAgent):**
  획득한 월드 좌표를 `NavMeshAgent`의 목적지로 설정합니다. 이를 통해 유닛은 **NavMesh** 데이터를 기반으로 장애물을 자동으로 회피하며 최적의 경로로 이동합니다.
