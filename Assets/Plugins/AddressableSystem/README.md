# SandyAddressable

Unity Addressable Asset System을 쉽게 사용할 수 있도록 도와주는 유틸리티 패키지입니다.

## 주요 기능

- Addressable 에셋 그룹 관리
- 씬 로딩 관리
- 에셋 로딩 및 언로딩 자동화
- 에셋 캐싱 시스템

## 설치 방법

1. Unity Package Manager에서 "Add package from git URL" 선택
2. 다음 URL 입력: `https://github.com/[repository-url].git`

## 사용 방법

1. Addressable System Config 생성
   - Project 창에서 우클릭
   - Create > Sandy > Addressable System Config 선택

2. Addressable 그룹 설정
   - Windows > Asset Management > Addressables > Groups 메뉴에서 그룹 생성
   - Addressable System Config에서 InitializeGroupData() 호출하여 그룹 정보 초기화

3. 코드에서 사용
```csharp
// 에셋 로드
var asset = await AddressableSystem.LoadAssetAsync<GameObject>("assetAddress");

// 씬 로드
await AddressableSystem.LoadSceneAsync("sceneAddress");

// 에셋 언로드
AddressableSystem.ReleaseAsset("assetAddress");
```

## 시스템 요구사항

- Unity 2020.3 이상
- Addressable Asset System 패키지 설치 필요

## 버전 정보

- 현재 버전: 0.2.0
- Unity 버전: 6000.0

## 라이센스

MIT License 
