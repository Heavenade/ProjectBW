# 흑과 백
### 세종대학교 게임 제작 동아리 '판도라 큐브' 장기 프로젝트

'흑과 백'은 세종대학교 게임제작동아리 판도라 큐브의 Team.Luminous 팀에 의해 제작됐습니다.


## 개요

게임 '흑과 백'은 추리/어드벤처 장르의 2D 흑백 게임입니다. 흑과 백이라는 게임의 이름처럼, 해당 게임은 흑과 백이라는 큰 주제를 가지고 흘러갑니다. 사람이 가지고 있는 어두운 면과 겉으로 뱉어내는 밝은 면, 그 이면 사이에서 발생하는 뒤틀린 감정들로 인해 발생하는 사건이 주된 이야기입니다. 온 세상이 흑과 백으로 이루어진 게임 속에서 어떻게 진실을 밝힐 수 있을까요?

당신은 수사관 메르테가 되어 한 연쇄살인 사건을 수사하게 됩니다.


## 개발 언어 및 환경

* Unity & C#


## 나의 역할

* 맵 배치 및 설정

* 대화 & 단서 시스템 구현

* 대화창 & 단서창(수첩) 구현

* 특정 조건에서 발생하는 이벤트들 구현

* 게임 저장 기능 및 게임 데이터 암호화 구현

* 튜토리얼 & 엔딩 & 크레딧 제작


## 새로운 시도 (참고 코드 추가 예정, 10/21)

* 처음으로 진행한 장기 프로젝트

* 튜토리얼을 제작함
  * 코루틴의 WaitUntil 기능을 이용하여, 플레이어가 특정 행동을 할 때까지 다른 행동을 못하게 함으로써 유저들을 유도함

* 대화 시스템 구현 

* 단서 시스템 구현

* 단서 정리 시스템 구현
  * 유저가 획득한 단서의 수에 따라 길이가 늘어나도록 양피지 길이를 동적으로 조정함
  * 보너스 단서를 위한 애니메이션을 제작하면서, 디테일하게 에니메이션을 설정해 볼 수 있었음.

* 기획자와 프로그래머 모두 효율적으로 작업할 수 있는 csv 파일 형식 사용
  * 이 과정에서 csv 파일 데이터를 파싱하는 클래스를 설계하고, 구현해볼 수 있었음
  * Dictionary와 List 자료구조를 활용

* 게임 데이터를 저장할 때, 로딩 화면을 처음으로 적용
  * 로딩 화면을 보여주면서, 백그라운드로 저장 작업을 함으로써 유저들이 렉걸린 것 처럼 느끼지 않게 함

* 게임 데이터를 암호화 하여 저장
  * SHA-256 방식과 Salt를 혼합하여 암호화를 진행함 (오픈소스를 커스텀해서 사용함)

* 화면에 나타나지 않는 게임 맵이 컴퓨터 자원을 얼마나 차지하고 있는지 알기 위해서, Unity에서 지원하는 Profiler를 이용해봤음


## 트레일러
* 링크 오른쪽 클릭 -> 새 탭에서 링크 열기

https://youtu.be/HJkVgN9CUjY


## 인게임 영상
### 타이틀 및 프롤로그

https://youtu.be/pLjz50qBpXY 

### 튜토리얼

https://youtu.be/1TjnSxnWzsA

### 여러 기능

https://youtu.be/OxmQziPwGT4

### 사건 3 플레이

https://youtu.be/o2dEq7eBfjw

### 이벤트

https://youtu.be/ZlNIsK5YHo0


## 직접 플레이 해보기

'흑과 백'을 해보고 싶은 마음이 생기셨나요?

'흑과 백' 게임 파일은 아래의 링크에서 다운받을 수 있습니다.

https://drive.google.com/file/d/1WwcFHHDRCjeMBob8Sog3T8CSw1sa3l7t/view
