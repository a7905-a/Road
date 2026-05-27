[![Unity](https://img.shields.io/badge/Unity-6000.3.2f1-000000?style=flat-square&logo=Unity&logoColor=white)](#)
[![language](https://img.shields.io/badge/language-C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)](#)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4?style=flat-square&logo=windows&logoColor=white)](#)

⭐ 쿼터뷰 3D 유니티 프로젝트입니다.


## ⭐ 주요 기능 및 아키텍처

### A. 요구사항 복잡도에 따른 이원화된 상태 머신(FSM) 아키텍처 설계
단순한 상태 전환이 필요한 객체와 복잡한 판단 로직이 필요한 객체의 특성을 분리하여, 두 가지 다른 형태의 상태 관리 패턴을 혼용해 시스템을 설계했습니다.

| 구분 | 이벤트/시각적 피드백 중심 객체(ex. 사용자 컨드롤 대상) | 복잡한 비즈니스 로직 중심 객체(ex. 자율 행동 AI) |
| :--- | :--- | :--- |
| **방식** | **상태-시각화 결합형 Animator-Driven FSM** | **순수 객체지향 상태 패턴 Interface-based State Pattern** |
| **구조** | 내장 상태 제어기(Animator Controller) 활용 | C# 다형성 및 인터페이스 (`IState`) 활용 |
| **선택 이유** | 사용자 입력에 따른 즉각적인 상태변화와 시각적 피드백의 완벽한 동기화가 가장 중요했기 때문에 선택. | 다중 조건 판별 및 로직의 깊이가 깊어, 상태 로직과 뷰(View)를 분리하여 개방-폐쇄 원칙을 확보하기 위해 선택. |
| **기대 효과** | `상태 머신 동작 StateMachineBehaviour`를 통해 각 상태의 로직을 독립적인 스크립트로 관리하여 유지보수성 상승. | 새로운 행동 패턴 추가 시 기존 코드를 수정하지 않고 클래스 추가만으로 확정이 가능한 유연한 구조 확보. |

**애니메이터 기반 Animator-Driven FSM**

<img width="500" height="400" alt="image" src="https://github.com/user-attachments/assets/fe080bc1-e042-4891-af5f-bfe49b38e6df" />

<img width="500" height="400" alt="image" src="https://github.com/user-attachments/assets/7fd56553-b42e-41ec-bb30-2bc51e66e0af" />


**인터페이스 기반 Interface-based State Pattern**

<img width="350" height="200" alt="image" src="https://github.com/user-attachments/assets/d004f850-c373-4629-bfb2-46b10645d2d8" />
<img width="550" height="400" alt="image" src="https://github.com/user-attachments/assets/78aff7f7-302f-4f92-ae75-79fc458a7b78" />

<br>

### B. 데이터 무결성을 위한 방어적 프로그래밍
프로젝트 특성상 동적 데이터(객체)의 생성과 소멸이 빈번하게 발생합니다. 메모리 참조 오류나 외부 클래스의 무분별한 데이터 변조를 차단하기 위해 캡슐화 원칙을 준수했습니다.

#### IReadOnlyList를 활용한 리스트 보호
중앙 데이터 관리 매니저가 외부 클래스에 리스트 데이터를 제공할 때, 데이터 읽기만 가능하고 수정은 불가능한 IReadOnlyList<T> 타입으로 캐스팅하여 반환합니다. 이를 통해 의도치 않은 데이터 오염(Add/Clear 등)을 방지합니다.

```csharp
// [Before] public List는 외부에서 마음대로 Add/Clear가 가능하여 위험함
// public List<GameObject> AllUnits; 

// [After] 내부는 List로 관리하되, 외부는 읽기 전용으로만 공개
[SerializeField] private List<GameObject> _allUnits = new List<GameObject>();
public IReadOnlyList<GameObject> AllUnits => _allUnits;
```

<br>

### C. 공간 데이터 처리 및 최적 경로 탐색 시스템 구축
2D/3D 공간상에서 사용자의 마우스 입력을 실제 월드 좌표로 치환하고, 동적 장애물을 회피하며 목표 지점까지 도달하는 길찾기 알고리즘을 시스템에 통합했습니다.

* **공간 좌표 맵핑 (Raycast):**
  카메라 뷰포트에서 스크린 좌표로 광선(Ray)을 투사하여, 물리 엔진이 지정된 바닥면(Layer)과의 충돌 지점을 정밀하게 연산해 3차원 월드 좌표(Vector3)로 변환합니다.
* **A 알고리즘 기반 이동 제어 (NavMesh):**
  도출된 월드 좌표를 에이전트의 목적지로 할당합니다. 엔진에 내장된 내비게이션 메쉬(NavMesh) 데이터를 바탕으로 최단 거리를 실시간으로 연산하며, 런타임 중 변화하는 환경을 능동적으로 회피하는 이동 로직을 구현했습니다.
  
![CursorMove](https://github.com/user-attachments/assets/d491411b-a06b-4a2f-9a5a-0db65425aa95)
<img width="1200" height="600" alt="image" src="https://github.com/user-attachments/assets/8c1b003e-a25c-4e4e-b01c-f7f7ad3c5457" />

