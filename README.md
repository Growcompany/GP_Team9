# Abyssal Cave

Unity 2D 액션 게임. 게임프로그래밍 수업 4인 팀 프로젝트 (2024.10 ~ 2024.12).

원본 리포: [zzinnu/GP_Team9](https://github.com/zzinnu/GP_Team9)

- 보스전 플레이 영상: https://youtu.be/GUGpDTAz6O4
- Unity 2023.2.20f1 · Cinemachine 3.0.1
- PC·모바일 양쪽 빌드. 씬: `MainMenu`, PC용 `SampleScene`·`BossScene`, 모바일용 `SampleScene Mobile`·`BossScene Mobile`

## 담당 (이진환, @Growcompany)

| 영역 | 내용 | 코드 |
|---|---|---|
| 2.5D 보스맵 | 3D 성곽 에셋 위에 BoxCollider2D·Rigidbody2D를 배치해 기존 2D 판정을 그대로 유지. Cinemachine 카메라가 Dolly Spline을 따라 이동 | `BossScene/` |
| 보스씬 흐름 | 입장 → 대화·음성(RPGTalk 스니펫 확장) → RoomPlay 활성화·UI 복원 → 처치 시 Result UI | `BossEntrance.cs`, `BossTalk.cs`, `ChangeRoom.cs` |
| 보스 패턴 | 지면 공격(병렬 루프) + 레이저·돌진·텔레포트·연속 돌진(가중치 2:3:1:1 랜덤). 경고 오브젝트와 판정 오브젝트를 분리하고 큰 패턴은 `isAttacking` 플래그로 직렬화 | `Monsters/Boss/` |
| 일반 몬스터 AI | 거리 기반 탐지·순찰(벽·낭떠러지 레이캐스트)·추적·공격, 스프라이트 프레임에 맞춰 PolygonCollider2D 재생성, 무적 프레임·사망 단일 처리·EXP 드랍 | `Monsters/MonsterController.cs` + 파생 4종 |
| 미니맵 | 별도 레이어를 Orthographic 카메라로 찍어 RenderTexture → RawImage | 씬 설정 (`SampleScene Mobile`) |

## 코드 위치

```
Assets/+---Code/I +---Scripts/
├─ II +---Monsters/
│  ├─ MonsterController.cs               공통 몬스터 AI
│  ├─ BatController.cs 외 3종             파생: 애니메이션 이름·경험치·순찰 여부만 정의
│  └─ Boss/
│     ├─ BossController.cs               보스 패턴 코루틴 전체
│     ├─ AttackWarning.cs                경고 표시 (노랑→빨강, 알파 30~100% 점멸)
│     ├─ AttackDamage.cs                 판정 트리거 (Player 태그)
│     ├─ Attack4_Fire.cs / Boss_Fire.cs  화염 장판 판정·수명
│     └─ Attack3Warning.cs / Hitted_Effect.cs
├─ II +---BossScene/
│  ├─ BossTalk.cs / ChangeRoom.cs
│  └─ III +---Camera/BossRoomCamera.cs
└─ II +---Environment/BossEntrance.cs
```

## 브랜치

- `main`: 팀 최종본
- `JinHwan`: 제 작업 브랜치
- 제 커밋만 보기: https://github.com/Growcompany/GP_Team9/commits/main/?author=Growcompany

플레이어 이동·스킬, UI·튜토리얼·EXP 연출, 맵 오브젝트는 팀원 담당입니다.
