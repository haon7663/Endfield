# SandyPooling

Unity 프로젝트를 위한 고성능 오브젝트 풀링 시스템입니다. 게임 오브젝트의 생성과 파괴를 최적화하여 성능을 향상시킵니다.

## 주요 기능

- 오브젝트 풀링 관리
- 자동 업데이트/레이트업데이트 처리
- 메모리 최적화
- 풀 크기 제한 설정
- 컴포넌트 캐싱 지원

## 설치 방법

1. Unity Package Manager에서 "Add package from git URL" 선택
2. 다음 URL 입력: `https://github.com/[repository-url].git`

## 사용 방법

1. 풀링할 오브젝트 설정
```csharp
public class YourPoolableObject : PoolingBase
{
    // 필요한 컴포넌트 및 로직 구현
}
```

2. 풀링 컨트롤러 사용
```csharp
// 오브젝트 생성
var pooledObject = poolingController.GetPoolable<YourPoolableObject>(
    "poolKey",
    prefab,
    spawnPosition,
    defaultCapacity: 10,
    maxSize: 100
);

// 오브젝트 반환
poolingController.ReturnPoolable(pooledObject);
```

## 주요 클래스

### PoolingController
- 오브젝트 풀링의 핵심 컨트롤러
- 풀 생성 및 관리
- 업데이트/레이트업데이트 처리

### PoolingBase
- 풀링 가능한 오브젝트의 기본 클래스
- 상속하여 커스텀 풀링 오브젝트 구현

## 시스템 요구사항

- Unity 2020.3 이상
- SandyToolkit.Core 패키지 필요

## 버전 정보

- 현재 버전: 0.1.0
- Unity 버전: 6000.0

## 라이센스

MIT License
