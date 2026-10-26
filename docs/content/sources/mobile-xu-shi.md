# 徐氏·移动版

> 此版本与 `classic:xu-shi` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，吴、女、3体力，乱世英杰2017；完整引用现有 `classic:wengua` 与 `classic:fuzhu`，不复制技能定义或运行时。

来源：[移动官网227](https://www.sanguosha.cn/pc/hero-detail-227.html)、[移动BWIKI徐氏，固定版本35633](https://wiki.biligame.com/msgs/index.php?title=徐氏&oldid=35633)、[问卦的徐氏入口](https://wiki.biligame.com/msgs/问卦/徐氏)、[其他角色入口](https://wiki.biligame.com/msgs/问卦/其他角色)与[伏诛](https://wiki.biligame.com/msgs/伏诛)。人物普通经典栏列出实际礼包获取方式，未混用国战或欢乐版本。

问卦按每位实际出牌角色分别限次，真实交入手牌后由徐氏决定是否将该实体置于牌堆顶或底，双方再从另一端各取得一张。自己发动跳过交牌，由同一角色从另一端取得一张；现有机制按实际参与者去重。HE实体、原生移牌观察与已付费恢复沿用共享实现。

伏诛按男性角色结束阶段、牌堆数不大于徐氏体力十倍判定，依次使用真实牌堆中的杀，最多为初始游戏参与人数，最后洗牌。官网较旧正文写“场上人数”，当前移动人物与技能页明确为游戏角色数。本实现采用当前移动专页，与已有 `DeckSlashSequence(..., _playerCount)` 一致，不改为存活人数，也不改变经典默认语义。

纯内容引用由共用加载、注册与机制检查覆盖；批次复用现有 `Fame 2017 Xu Shi foreign equipment AI replay`，不新增重复人物夹具。

经典立绘来自[官网39000.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/39000.jpg)，本地 `official-mobile-xu-shi.jpg`，图鉴分组 `fame-7`。
