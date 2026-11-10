# ⛏️ Drill Idle (데스크탑 방치형 공장 시뮬레이션)
> **"나의 타이핑 작업이 공장의 성장으로 연결되는 마이크로-브레이크(Micro-Break) 시뮬레이션 게임"**


## 📖 게임 소개
**Drill Idle**은 기존 공장 시뮬레이션 게임(새티스팩토리, 팩토리오 등)의 복잡한 진입 장벽과 높은 플레이 타임 요구를 **'방치형'** 요소와 결합하여 풀어낸 데스크탑 전용 게임입니다.

화면의 한편에 작게 띄워두고(최소화 위젯 모드 지원), 사용자가 일상적인 **'키보드 타이핑'**을 할 때마다 공장의 시설이 가동되며 자원을 생산합니다. 단 몇 십 초의 짧은 시선 이동(Micro-Break)만으로 공장의 성장을 확인하고, 지루한 작업의 동기를 긍정적으로 재충전할 수 있습니다.

* **개발 기간:** [개발 기간 작성, 예: 2023.09 ~ 2023.12]
* **참여 인원:** 3인 (담당 역할: 클라이언트 개발 및 핵심 기획)
* **개발 환경:** Unity 3D, C#, Windows API, Addressables, DOTween

---

## 🚀 주요 기능 (Features)

### 1. 즉각적인 성취 피드백: 타이핑 = 공장 가동
사용자의 키보드 입력(Windows API Key Hooking)을 감지하여 게임 내 동력으로 치환합니다. 게임이 백그라운드에 있어도 타이핑을 치면 각 시설이 즉각적으로 작동하여 긍정적인 피드백을 제공합니다.


<img width="400" alt="키입력을동력으로" src="https://github.com/user-attachments/assets/88916ce7-c622-4caf-a3fe-170c1b492f54" />


### 2. 그리드 기반의 직관적인 건설 시스템
복잡성을 덜어낸 상부/하부 그리드 시스템을 통해, 수집한 자원으로 채굴 시설과 가공 시설을 직관적으로 건설하고 철거할 수 있습니다.

<img width="400" alt="건설" src="https://github.com/user-attachments/assets/dd8348f8-ff81-42c2-88fd-9c45d21b9513" />
<img width="400" alt="철거" src="https://github.com/user-attachments/assets/e7aa0292-575c-4137-b20b-66584c465bd5" />




### 3. 엔진 합성 및 강화 시스템
자원을 소모하여 코어(Core)를 중심으로 연구소를 짓거나, 하위 레벨의 엔진 2개를 합성하여 상위 레벨의 엔진(초록, 파랑, 보라 등)을 개발하여 공장의 생산 효율을 극대화할 수 있습니다.


<img width="400" alt="엔진 합성" src="https://github.com/user-attachments/assets/38b6a9b4-046c-46d5-8561-50ad6384a240" />
<img width="400" alt="연구" src="https://github.com/user-attachments/assets/52eb8e4c-d595-4a59-bf32-018c650c0343" />





### 4. 작업의 습관화: 캘린더 (작업 기록)
하루 동안 이 게임을 켜둔 시간과 타이핑을 입력한 횟수가 캘린더 위젯에 저장됩니다. "내가 오늘 이만큼 일했구나"를 정량적으로 확인하며 작업의 보람감을 시각화합니다.

<img width="207" alt="캘린더" src="https://github.com/user-attachments/assets/0277be3c-f595-4d0c-bc71-4425eb4d6ca6" />


---

## 🛠 기술 스택 (Tech Stack)
Client
- Engine: Unity 6
- UI & Animation: DOTween
- Data Management: Addressables, Scriptable Object, CSV Parsing
- Save System: Easy Save 3 (ES3)

System & OS
- OS API: Windows API (Keyboard HookCallback)
