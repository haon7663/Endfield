# SandyToolkitCore

Unity 프로젝트를 위한 핵심 유틸리티 및 프레임워크 패키지입니다. 게임 개발에 필요한 기본적인 기능들을 제공합니다.

## 주요 기능

### 기본 프레임워크
- 컨트롤러 기반 아키텍처
- 초기화 시스템
- 업데이트 시스템
- 서비스 관리

### 유틸리티
- 컴포넌트 캐싱 시스템
- 클래스 컨테이너
- 안전하지 않은 타입 변환 도우미
- 딕셔너리 확장 메서드

### 인터페이스
- IInitializable: 초기화 가능한 객체 인터페이스
- IUpdate: 업데이트 가능한 객체 인터페이스
- IUpdateLoop: 업데이트 루프 인터페이스

## 설치 방법

1. Unity Package Manager에서 "Add package from git URL" 선택
2. 다음 URL 입력: `https://github.com/[repository-url].git`

## 사용 방법

1. 기본 컨트롤러 구현
```csharp
public class YourController : BaseController
{
    public override void OnInitialize()
    {
        // 초기화 로직
    }

    public override void OnUpdate()
    {
        // 업데이트 로직
    }
}
```

2. 컴포넌트 캐싱 사용
```csharp
// 컴포넌트 가져오기
var component = CachedComponents.GetComponent<YourComponent>(gameObject);

// 컴포넌트 추가
CachedComponents.AddComponent(gameObject, component);
```

3. 클래스 컨테이너 사용
```csharp
var container = new ClassContainer<int, YourClass>();
container.AddElement(id, instance);
var element = container.GetElement(id);
```

## 주요 클래스

### BaseController
- 모든 컨트롤러의 기본 클래스
- 초기화 및 업데이트 시스템 제공
- 서비스 관리 기능

### CachedComponents
- 컴포넌트 캐싱 시스템
- 성능 최적화를 위한 컴포넌트 재사용
- 메모리 관리 최적화

### ClassContainer
- 제네릭 타입 컨테이너
- ID 기반 요소 관리
- 스레드 안전한 접근

## 시스템 요구사항

- Unity 2020.3 이상
- .NET 4.x 이상

## 버전 정보

- 현재 버전: 0.1.0
- Unity 버전: 6000.0

## 라이센스

MIT License
