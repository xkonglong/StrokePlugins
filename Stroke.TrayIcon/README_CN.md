# Stroke.TrayIcon

为 [Stroke](https://github.com/poerin/Stroke) 鼠标手势引擎提供系统托盘功能的插件。它在通知区域显示图标，提供暂停 / 恢复手势及退出程序的控制入口。

## 目录

- [功能特性](#功能特性)
- [运行要求](#运行要求)
- [使用方法](#使用方法)
    - [手势脚本](#手势脚本)
- [许可证](#许可证)

## 功能特性

- **托盘图标**：在系统通知区域显示 Stroke 图标。
- **暂停 / 恢复手势**：左键单击图标或使用右键菜单可暂停手势检测，再次执行相同操作即可恢复。
- **退出 Stroke**：右键单击图标并选择“退出”，彻底关闭手势引擎。

## 运行要求

- Stroke 引擎。
- .NET Framework 4.8 运行时。

## 使用方法

### 手势脚本

插件仅提供一个公开方法 `TrayIcon.SetVisibility(bool visible)`，用于显示或隐藏托盘图标。

以下切换脚本利用 `Base.Data` 记录图标可见状态。执行该手势，即可在显示与隐藏之间切换。

```csharp
if (!Base.Data.ContainsKey("tray"))
{
    Base.Data["tray"] = true;
    TrayIcon.SetVisibility(true);
}
else
{
    bool current = (bool)Base.Data["tray"];
    TrayIcon.SetVisibility(!current);
    Base.Data["tray"] = !current;
}
```

## 许可证

基于 MIT 许可证开源。