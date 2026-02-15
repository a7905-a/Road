# 🎮 [Project Road]

<br>

## 📌 1. 프로젝트 개요 (Overview)
- **개발 기간:** 2024.00.00 ~ 2024.00.00 (약 O주)
- **개발 인원:** 1인 개인 프로젝트
- **사용 엔진:** Unity 2022.3.xx (LTS)
- **사용 언어:** C#
- **핵심 기술:** `Hybrid FSM`, `IReadOnlyList`, `NavMesh`, `Raycast Interaction`

<br>

## 🎥 2. 시연 영상 (Demo)
![In-Game Screenshot](https://via.placeholder.com/800x400?text=Please+Upload+Your+Game+Screenshot+Here)

* 📺 **[유튜브 시연 영상 보러가기](https://youtube.com/your_link_here)**

<br>

## 🛠️ 3. 기술적 의사결정 (Technical Decision)

### A. 하이브리드 FSM (Hybrid Finite State Machine) 설계
유닛의 역할과 기획 의도에 따라 두 가지 다른 FSM 방식을 혼용하여 최적의 효율을 냈습니다.

| 구분 | 플레이어 유닛 (Player) | 적 유닛 (Enemy AI) |
| :--- | :--- | :--- |
| **방식** | **Animator-Driven FSM** | **Interface-based State Pattern** |
| **구조** | Unity Animator Controller 활용 | C# Interface (`IState`) 활용 |
| **선택 이유** | 유저 입력에 따른 **즉각적인 애니메이션 반응**과 시각적 상태 관리가 중요함. | 추격, 정찰 등 **복잡한 판단 로직**이 필요하며, 애니메이션과 로직을 분리하여 확장성(OCP)을 확보해야 함. |
| **기대 효과** | `StateMachineBehaviour`를 통해 상태 진입/탈출 로직을 모듈화하여 관리. | 새로운 행동 패턴 추가 시 기존 코드를 수정하지 않는 유연한 구조 확보. |

<br>

### B. 데이터 무결성을 위한 방어적 프로그래밍 (Data Integrity)
RTS 장르 특성상 다수의 유닛이 생성되고 파괴되므로, **데이터 오염 방지**를 위해 캡슐화를 강화했습니다.

#### 1. IReadOnlyList를 활용한 리스트 보호
유닛 관리 매니저(`UnitSelectionManager`)가 외부 클래스에 리스트를 제공할 때, 수정 불가능한 `IReadOnlyList<T>` 인터페이스를 반환합니다.

```csharp
// [Before] public List는 외부에서 마음대로 Add/Clear가 가능하여 위험함
// public List<GameObject> AllUnits; 

// [After] 내부는 List로 관리하되, 외부는 읽기 전용으로만 공개
[SerializeField] private List<GameObject> _allUnits = new List<GameObject>();
public IReadOnlyList<GameObject> AllUnits => _allUnits;
