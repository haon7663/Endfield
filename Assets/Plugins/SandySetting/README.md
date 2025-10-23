# SandySetting

Unity 프로젝트를 위한 설정 관리 시스템입니다. 게임의 다양한 설정을 중앙에서 관리하고 저장할 수 있습니다.

## 주요 기능

- 설정 모듈 기반 아키텍처
- 자동 모듈 검색 및 초기화
- 설정 데이터 영구 저장
- 오디오 설정 관리
  - 마스터 볼륨
  - BGM 볼륨
  - SFX 볼륨
  - 앰비언스 볼륨
- 모듈 기반 확장성

## 설치 방법

1. Unity Package Manager에서 "Add package from git URL" 선택
2. 다음 URL 입력: `https://github.com/[repository-url].git`

## 사용 방법

1. 설정 모듈 구현
```csharp
public class YourSettingModule : ISettingModule
{
    public void SettingModule_Initialize()
    {
        // 초기화 로직
    }

    public void SettingModule_Dispose()
    {
        // 정리 로직
    }
}
```

2. 오디오 설정 모듈 구현
```csharp
public class YourAudioModule : IAudioSettingModule
{
    public void Initialize(IAudioSetting settings)
    {
        // 오디오 설정 초기화
    }

    public void SetVolume(BusType busType, float volume)
    {
        // 볼륨 설정
    }
}
```

3. 설정 컨트롤러 사용
```csharp
// 오디오 설정 변경
settingController.AudioSetting.MasterVolume = 0.8f;
settingController.AudioSetting.BgmVolume = 0.7f;
settingController.AudioSetting.SfxVolume = 0.9f;
settingController.AudioSetting.AmbienceVolume = 0.6f;
```

## 주요 클래스

### SettingController
- 설정 관리의 핵심 컨트롤러
- 모듈 자동 검색 및 초기화
- 설정 데이터 저장/로드

### ISettingModule
- 설정 모듈 인터페이스
- 커스텀 설정 모듈 구현을 위한 기본 인터페이스

### IAudioSettingModule
- 오디오 설정 모듈 인터페이스
- 오디오 관련 설정 관리를 위한 인터페이스

## 시스템 요구사항

- Unity 2020.3 이상
- SandyToolkit.Core 패키지 필요
- UniTask 패키지 필요

## 버전 정보

- 현재 버전: 0.0.1
- Unity 버전: 6000.0

## 라이센스

MIT License
