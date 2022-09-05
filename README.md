# jukey17games.utilities.pausable

## 概要

UnityのtimeScaleに依存しないポーズ処理を実現するための機能を提供します。

## インストール

Unity標準のPackage Managerに対応しています。

Package Managerウィンドウから `Add package from git URL...` を選択し、下記のURLを指定してください。

```
https://github.com/jukey17/jukey17games.utilities.pausable.git
```

または `manifest.json` の `dependencies` の項目に下記の記述を追加してください。

```json:manifest.json
{
  "dependencies": {
    "com.jukey17games.utilities.pausable": "https://github.com/jukey17/jukey17games.utilities.pausable.git"
  }
}
```

## サンプル

https://github.com/jukey17/jukey17games.utilities.pausable.examples にてサンプル実装が用意されています。

## 詳細

### Pausable

本パッケージではポーズ可能な要素を **Pausable** と表現し、コード上では `IPausable` インターフェイスを実装しているクラスがそれに該当します。

```csharp
public class CharacterController : IPausable
{
    public void OnPaused()
    {
        // 一時停止に必要な処理
    }
    
    public void OnResumed()
    {
        // 一時停止からの再開に必要な処理
    }
}
```

### PausableSystem

`PausableSystem` は、本パッケージにおけるコアとなるポーズ制御を行える機構です。

前述した `IPausable` インターフェイスを実装した任意のクラスを登録して一括でポーズ制御(Pause/Resume)が可能です。

```csharp
public class PausableA : IPausable { ... }
public class PausableB : IPausable { ... }
public class PausableC : IPausable { ... }

IPausableSystem pausableSystem = new PausableSystem();

// ポーズ制御可能なインスタンスを登録
pausableSystem.Register(new PausableA());
pausableSystem.Register(new PausableB());
pausableSystem.Register(new PausableC());

// 一括で一時停止する
pausableSystem.Pause();
```

### GroupPausable

**GroupPausable** は前述した **Pausable** 機能にビットフラグによるグループ化を拡張した機構です。

`GroupPausableSystem` を利用することでグループを指定した部分的なポーズ制御を実現しています。

```csharp
public class PausableA : IPausable { ... }
public class PausableB : IPausable { ... }
public class PausableC : IPausable { ... }

// ビットフラグでグループを用意する
public enum Groups
{
    A = 1 << 0,
    B = 1 << 1,
    C = 1 << 2,
}

IGroupPausableSystem pausableSystem = new GroupPausableSystem();

// ポーズ制御可能なインスタンスをグループで登録
pausableSystem.Register(new PausableA(), (int) Groups.A);
pausableSystem.Register(new PausableB(), (int) Groups.B);
pausableSystem.Register(new PausableC(), (int) Groups.C);

// Aだけを一時停止する
pausableSystem.Pause((int) Groups.A);
```

### PauseSwitcher

実際に `Pause/Resume` 命令を実行する機能を **PausableSwitcher** と表現し、コード上では `IPausableSwitcher` インターフェイスを実装しているクラスがそれに該当します。

前述した `PausableSystem`, `GroupPausableSystem` はこのインターフェイスを実装しています。

また、このインターフェイスを実装してUnityの標準コンポーネントを制御できるようにしたクラスも用意されています。
※現在用意されているクラスは[こちら](./Runtime/Switcher)を参照ください。

### Timer

`System.Diagnosics.Stopwatch` クラスを利用した時間制限を行えるタイマー機構が用意されています。
