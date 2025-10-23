# CustomEasing

Unity용 커스텀 이징(Easing) 함수 라이브러리입니다. 다양한 애니메이션 효과를 구현할 수 있는 이징 함수들을 제공합니다.

## 주요 기능

- 30가지 이상의 다양한 이징 함수 제공
- 모든 함수는 정적(static) 메서드로 구현되어 있어 쉽게 사용 가능
- 각 함수의 미분(Derivative) 버전도 제공
- Unity의 Mathf 클래스와 호환

## 지원하는 이징 함수

- Linear (선형)
- Spring (스프링)
- Quad (2차)
- Cubic (3차)
- Quart (4차)
- Quint (5차)
- Sine (사인)
- Expo (지수)
- Circ (원형)
- Bounce (바운스)
- Back (백)
- Elastic (탄성)

각 함수는 다음과 같은 변형을 제공합니다:
- EaseIn (시작점에서 가속)
- EaseOut (종료점에서 감속)
- EaseInOut (시작과 종료에서 가속/감속)

## 사용 방법

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // 시작값, 종료값, 진행도(0~1)
        float result = CustomEasing.EaseInQuad(0f, 1f, 0.5f);
        
        // 이징 함수를 직접 가져와서 사용
        CustomEasing.Function easingFunc = CustomEasing.GetEasingFunction(CustomEasing.Ease.EaseInQuad);
        float result2 = easingFunc(0f, 1f, 0.5f);
    }
}
```

## 미분 함수 사용

각 이징 함수의 미분 버전은 'D' 접미사가 붙은 메서드로 제공됩니다:

```csharp
float derivative = CustomEasing.EaseInQuadD(0f, 1f, 0.5f);
```

## 라이센스

이 프로젝트는 MIT 라이센스 하에 배포됩니다.
