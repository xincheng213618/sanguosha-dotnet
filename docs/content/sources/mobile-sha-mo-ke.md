# 移动版沙摩柯

> 此版本与 `classic:sha-mo-ke` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-26。身份 `mobile:sha-mo-ke`，人物 `character:sha-mo-ke`，蜀、男、4 体力，SP 武将组9。

来源：[移动版官网武将页293](https://www.sanguosha.cn/pc/hero-detail-293.html)、[移动版 BWIKI 人物正文（oldid=37555，页面更新2026-03-17）](https://wiki.biligame.com/msgs/index.php?title=沙摩柯&oldid=37555)。官网包含身份版，BWIKI 当前经典身份列明将魂或活动兑换的获取方式，属于正式已上线人物。本身份采用普通移动版，不将其与国战权限或其他产品的图鉴分类混淆。

蒺藜在一个实际回合内，沙摩柯使用或打出的牌数达到当前攻击范围时，可以摸等同当前攻击范围数量的牌。官网与移动 BWIKI 现行经典文本一致，与本库 `classic:jili` 的完整实现相同，直接引用该技能 ID。原程序分别在实际用牌提交与打出响应接受窗口统计 `cardsUsedOrRespondedThisTurn`，与当前攻击范围比较并据此摸牌；没有额外添加每回合只能发动一次的限制，攻击范围变化仍可影响之后的匹配。

本身份只新增 Content 注册，沿用已有技能、真实摸牌、移动子窗与原帧恢复，不复制技能 JSON、Runtime 或重复人物夹具。移动版武将包为 SP，图库组采用 `sp`。

肖像键 `mobile-sha-mo-ke`；[官网经典原画47500](https://www.sanguosha.cn/storage/uploads/images/skins/47500.jpg)。素材与图鉴由统一批次接线。
