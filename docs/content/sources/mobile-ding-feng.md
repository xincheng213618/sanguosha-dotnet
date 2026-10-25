# 移动版丁奉

核对日期：2026-10-26。正式身份 `mobile:ding-feng`，吴、男性、四体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-ding-feng`。

[移动版官网丁奉87](https://www.sanguosha.cn/pc/hero-detail-87.html)完整HTML含默认隐藏的奋迅技能文本；身份和国战两个面板均为：短兵让杀多选择一名距离为1的目标，奋迅出牌阶段限一次、弃置一张牌并选其他角色，本回合与其距离视为1。本项目采用此身份文本，不含现有OL短兵的双闪要求及OL奋迅的结束阶段弃牌债。

短兵复用已注册的原生杀目标追加能力，只去除OL独有的双闪policy。奋迅先由通用 `discardSelected` 支付一张手牌或装备，完整返回其移动子流程后在原技能帧发出一条实际回合距离许可；规则查询仅对此原主体与目标设距离1，保持反方向及其他目标原有距离。许可沿公开标量事件派生，实际回合结束到期，不增加待决容器、存档版本或隐藏旁路状态。

独立画像为官网首张[经典形象12600](https://www.sanguosha.cn/storage/uploads/images/skins/12600.jpg)，资源文件 `src/CardGame.Wpf/Assets/official-mobile-ding-feng.jpg`；由批次统一绑定和核验。
