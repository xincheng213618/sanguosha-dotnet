# 严颜·移动版

核查日期：2026-10-26。普通移动身份，蜀、男、4体力，神话再临阴；完整引用已注册的 `classic:juzhan`，没有复制技能、状态或运行时。

来源：[移动官网249](https://www.sanguosha.cn/pc/hero-detail-249.html)、[移动BWIKI严颜固定版本35823](https://wiki.biligame.com/msgs/index.php?title=严颜&oldid=35823)（2026-01-10）、[拒战普通严颜固定版本40028](https://wiki.biligame.com/msgs/index.php?title=拒战/严颜&oldid=40028)（2026-08-02）。当前人物普通经典栏列出500将魂合成/招募获得；不是国战、界限突破或线下重制版。

官网与当前移动专页的普通拒战一致：初始为阳，成为其他角色使用杀的目标后可令双方各摸1，再在本回合禁止该使用者对自己用牌；阴面在自己使用杀指定目标后可取得其一张牌，再在本回合禁止自己对该目标用牌。发动后在两面之间转换，不按回合开始或结束自动重置。

已有共享规则 `src/Characters/_shared/Skills/classic-card-action-skills.rules.json` 的 `classic:juzhan` 声明普通、火、雷三种杀的真实目标窗口，以公开 `yin` 状态区分两面；阳面顺序为双方原生摸牌、有向回合目标禁止及转换，阴面使用目标原生选牌取得、有向禁止及转换。该完整技能ID沿用原生目标、移牌、状态与存档合同，不引用带第三次计数、初始阴或乘势收益的 `boundary:juzhan-mobile`。

本次为纯内容引用，由既有规则加载、内容注册、原生牌目标和有向目标策略检查覆盖，不新增重复人物夹具或测试快照。中央图库与素材由批次整合接入。

经典立绘来自[官网41600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/41600.jpg)，对应当前官网249经典首图；本地目标名 `official-mobile-yan-yan.jpg`，`portraitKey` 为 `mobile-yan-yan`，图鉴分组 `myth-yin`。
