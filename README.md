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
> **플렛폼**
>
> - 모바일
> 
>  **형상관리**
> - GitHub Desktop
> - Sourcetree

### 2. 참여 인원 및 역할 분담

> |                    이름                   |     역할      |
> | :---------------------------------------: | :-----------: |
> | [김양현](https://github.com/yangstar98) | 몬스터, 스테이지 스폰, 몬스터 및 배경 사운드(오디오믹서), 인트로, 자원|
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

### 4. 구현 기능

> 오브젝트
> - 플레이어
>> - 전사
>> - 스킬
> - 몬스터
>> - 슬라임
>> - 박쥐
>> - 거미
>> - 드래곤(파이어볼)
> - 아이템
>> - 코인
>>
> UI
> - Scene
>> - TitleScene : 인트로 배경(움직이는 구름), 로고, 게임 시작버튼
>> - MainScene : 인벤토리창,  장비창, 퀘스트창, 스탯업창, 뽑기창, 스킬슬롯 및 장착 창, 소리조절창, 게임오버창, 스테이지, 자원, 플레이어 HP, 데미지 텍스트, 조이스틱

### 5. 개발 일지

> - [티스토리](https://yangstar.tistory.com/category/%EB%B0%A9%EC%B9%98%ED%98%95%20%EA%B2%8C%EC%9E%84%20%ED%8C%80%20%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8%28Hero%20Idle%29)

### 6. 플레이 영상

> - [유튜브](https://youtu.be/JWQJ6-yybFE)

### 7. 기술 문서

> - https://docs.google.com/presentation/d/1SJ47OXmOFBczRXy3uMhe_4qY1k1nL5Dy_cxkUX4--zY/edit#slide=id.p




