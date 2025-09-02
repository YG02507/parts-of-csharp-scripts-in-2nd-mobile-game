# parts-of-csharp-scripts-in-2nd-mobile-game
This public repository showcases parts of C# scripts from my 2nd mobile game project. The scripts are designed to be self-descriptive, modular, and easy to maintain.

### 2nd 모바일 게임 프로젝트 C# 스크립트 포트폴리오

---

### 1. 개요

본 리포지토리는 Unity로 개발된 모바일 게임의 핵심 C# 스크립트들을 담고 있습니다. 견고하고 유지보수하기 쉬운 코드를 작성하는 것을 목표로 했습니다.

### 2. 포함된 파일 목록

* **`ExtendedButton.cs`**: Unity의 기본 `Button` 컴포넌트를 확장하여 독점적인 UI/UX 효과를 구현합니다.

* **`ExtendedToggle.cs`**: Unity의 기본 `Toggle` 컴포넌트를 확장하여 게임의 고유한 기능을 추가합니다.

* **`Kit.cs`**: 프로젝트 전반에 걸쳐 재사용되는 유틸리티 메서드, 상수, 열거형 등을 모아놓은 클래스입니다.

* **`PreferenceModule.cs`**: 게임의 진행 상황, 재화 등 주요 데이터를 `PlayerPrefs`를 활용하여 안전하게 저장하고 관리하는 역할을 담당합니다.

### 3. 기술적 특징 및 코딩 철학

* **자기 설명적(Self-Descriptive) 코딩 스타일**: 코드의 가독성과 유지보수성을 극대화하기 위해 주석에 대한 의존을 최소화했습니다. 변수, 메서드, 클래스 이름을 구체적이고 명확하게 작성하여 코드 자체가 자신의 역할을 설명하도록 설계했습니다.

* **객체 지향(Object-Oriented) 원칙 적용**:
    * **상속**: `ExtendedButton`과 `ExtendedToggle`은 Unity의 기본 클래스를 확장하여 기능을 추가했습니다.
    * **단일 책임 원칙(SRP)**: `Kit` 클래스는 유틸리티 기능만을, `PreferenceModule`은 데이터 관리 기능만을 담당하도록 분리하여 각 모듈의 응집도를 높였습니다.
    * **모듈화 및 의존성 관리**: 프로젝트 전반에 걸쳐 자주 사용되는 기능들을 `Kit` 클래스로 모듈화하여 코드 중복을 줄였습니다.

* **견고하고 효율적인 데이터 관리**: `PreferenceModule`은 `PlayerPrefs`의 한계를 극복하기 위해 `JsonUtility`를 사용해 복잡한 데이터 구조(`struct`)를 효율적으로 저장합니다. 이는 게임 데이터의 안정성을 보장하고, 향후 업데이트 시 데이터 구조를 유연하게 변경할 수 있게 합니다.

### 4. 면접관에게 전하고 싶은 메시지

본 스크립트들은 단순히 기능 구현을 넘어, 실제 프로젝트를 고려한 설계와 코드 품질에 대한 저의 철학이 담겨 있습니다. 코드를 통해 제가 어떤 문제를 어떻게 해결하고, 어떤 방식으로 협업할 수 있는 개발자인지 보여드리고 싶습니다.

궁금한 점이 있으시면 언제든지 질문해주세요.
