# SandyToolkit

SandyToolkit은 Unity 프로젝트를 위한 유틸리티 툴킷입니다. 게임 개발에 필요한 다양한 핵심 기능들을 제공합니다.

## 주요 기능

### 1. 애플리케이션 관리 (ApplicationManager)
- 게임의 전체적인 생명주기 관리
- 씬 전환 및 상태 관리
- 게임 설정 관리

### 2. 리소스 관리 (ResourceManageController)
- 게임 리소스의 효율적인 로딩 및 관리
- 메모리 최적화

### 3. 설정 관리 (SettingController)
- 게임 설정 관리
- 사용자 환경 설정 저장 및 로드

### 4. 오브젝트 풀링 (PoolingController)
- 게임 오브젝트의 재사용을 통한 성능 최적화
- 메모리 사용량 감소

## 설치 방법

1. Unity Package Manager를 통해 설치:
   - Package Manager 창을 엽니다 (Window > Package Manager)
   - "+" 버튼을 클릭하고 "Add package from git URL"을 선택
   - 저장소 URL을 입력

## 사용 방법

각 컨트롤러는 싱글톤 패턴으로 구현되어 있어 다음과 같이 접근할 수 있습니다:

```csharp
// 리소스 관리
ResourceManageController.Instance.LoadResource("resourcePath");

// 설정 관리
SettingController.Instance.SaveSetting("key", value);

// 오브젝트 풀링
PoolingController.Instance.GetObject("prefabName");
```

## 버전 정보

- 현재 버전: 0.2.0
- Unity 버전: 6000.0 이상

## 라이센스

이 프로젝트는 MIT 라이센스 하에 배포됩니다.

## 문의

개발자: Wise
