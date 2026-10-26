# 满宠（移动版）

> 此版本与 `classic:man-chong` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-25。普通移动身份 `mobile:man-chong`，魏、男性、3体力，完整复用 `classic:junxing`、`classic:yuce`。

- [移动官网武将125](https://www.sanguosha.cn/index.php/pc/hero-detail-125.html)：已读取包含隐藏御策的完整HTML。峻刑以任意非空手牌为代价，对方可以交出类别均异的一张手牌，否则翻面并摸原代价数量；御策是受伤后展示手牌，伤害来源弃异类别手牌或持有者回复一点。
- [移动 BWIKI 满宠](https://wiki.biligame.com/msgs/%E6%BB%A1%E5%AE%A0)，页面更新于2023-02-01：经典满宠的体力、两技能与官网一致，和界满宠分开列出。没有采用界峻刑“弃等量牌并失去体力”的分支。

逐节点核对现有 `category-challenge-skills.rules.json`：峻刑捕获原所选手牌、真实弃置，再由原目标私下选择类别与所有原代价均不同的手牌；拒绝后原生翻面，并按原冻结数量摸牌。原技能统一使用每实际出牌阶段一次配额。御策在真实每次伤害后只提供一次选择，先选并公开原手牌，再由实际来源选择异类别手牌；未支付时原生回复一点。两项规则相同，普通移动人物直接指向原技能 ID，没有复制 DSL、Runtime 或状态。

没有新增机制或人物专用 fixture。既有过滤器 `Mobile boundary Man Chong: turnover original draw count and per-damage Yuce` 保留原生翻面摸牌和共用御策检查；`skill executor resumes child resolution without repeating paid effects` 沿用原付款孩子续接检查。共享每阶段配额使用现有执行器与阶段账本。

立绘为[官网当前原画53200](https://www.sanguosha.cn/storage/uploads/images/skins/53200.jpg)，530×750，保存为 `official-mobile-man-chong.jpg`，未裁剪、重采样或重绘。SHA-256：`5657ced81c85b56d1231aba1fe77ea3dd1c5dac95f83fdccda1eba45fcd23d71`。
