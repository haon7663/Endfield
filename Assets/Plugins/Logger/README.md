# SandyLogger

Unity 프로젝트를 위한 고성능 로깅 시스템입니다. 콘솔과 파일 로깅을 지원하며, 비동기 처리를 통해 성능 저하를 최소화합니다.

## 주요 기능

- 다중 로그 레벨 지원 (Debug, Info, Warning, Error)
- 콘솔 및 파일 로깅 동시 지원
- 비동기 파일 쓰기로 성능 최적화
- 자동 로그 파일 관리
  - 최대 파일 수 제한 (기본값: 50개)
  - 최대 파일 크기 제한 (기본값: 10MB)
  - 보관 기간 제한 (기본값: 30일)
- 빌드 타입별 로그 레벨 자동 설정
  - 에디터: Debug 레벨
  - 릴리즈: Warning 레벨 이상

## 설치 방법

1. Unity Package Manager에서 "Add package from git URL" 선택
2. 다음 URL 입력: `https://github.com/[repository-url].git`

## 사용 방법

```csharp
// 로거 인스턴스 생성
private readonly SandyLogger _logger = new SandyLogger(typeof(YourClass));

// 로그 출력
_logger.Debug("디버그 메시지");
_logger.Info("정보 메시지");
_logger.Warning("경고 메시지");
_logger.Error("에러 메시지");

// 로그 레벨 설정
_logger.SetMinLogLevel(LogLevel.Info);

// 로깅 방식 설정
_logger.EnableFileLogging(true);    // 파일 로깅 활성화/비활성화
_logger.EnableConsoleLogging(true); // 콘솔 로깅 활성화/비활성화
```

## 로그 파일 위치

- 로그 파일은 `Application.persistentDataPath/Logs` 디렉토리에 저장됩니다.
- 파일명 형식: `game_log_yyyy-MM-dd_HH-mm-ss.txt`

## 시스템 요구사항

- Unity 2020.3 이상
- UniTask 패키지 필요

## 버전 정보

- 현재 버전: 0.0.1
- Unity 버전: 6000.0

## 라이센스

MIT License
