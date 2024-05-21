# 🎮[Unity 2D]Hero Idle

 > <img width="614" alt="HeroIdleTitle" src="https://github.com/yangstar98/Hero-Idle-portfolio/assets/167849650/2987c8e7-b407-423c-b6f9-64bed01859b7">

### 1. 프로젝트 정보

> **개발기간** : 2024.04.22 ~ 2024.05.17(약 4주)
>
> **개발 환경**
> - Unity 2022.3.25f1 LTS
> - C#
> - window 11
> - Visual Studio 2019
> 
>  **형상관리**
> - GitHub Desktop
> - Sourcetree

### 2. 참여 인원 및 역할 분담

> |                    이름                   |     역할      |
> | :---------------------------------------: | :-----------: |
> | [김양현](https://github.com/yangstar98) | 몬스터, 스폰 관리, 스테이지, 몬스터 및 배경 사운드(오디오믹서), 인트로, 자원|
> |  [구학균](https://github.com/GoNyGuI)   | 플레이어, 스킬, 플레이어 사운드 및 애니메이션 |
> |  [김재훈](https://github.com/JaerHoon)  | 장비, 인벤토리 내부 구현 및 UI, 플레이어 스탯 |

### 3. 사용 기술

| 기술 | 설명 |
|:---:|:---|
| 디자인 패턴 | ● **싱글톤** 패턴을 사용하여 Manager 관리 <br> ● **ScriptableObject**를 사용하여 게임 데이터를 에셋화하여 관리에 용이<br> ●**이벤트 액션**을 사용하여 느슨한 결합으로 스크립트를 연결하여 엔진의 부담을 줄임|
| 유한상태머신 | 객체의 상태를 **enum**으로 선언하며 애니메이션을 정수 파라미터로 연결  |
| Save | 일부 고정 데이터는 ScriptableObject를 게임 종료 시 저장할 데이터는 json으로 변환하여 관리 |
| Object Pooling | 자주 사용되는 객체는 Pool 관리하여 재사용(몬스터, 투사체, 코인) |
| 클래스 상속 | 중복되는 코드를 최소화하고 오버라이딩을 이용해 효율적인 코드 관리 |
