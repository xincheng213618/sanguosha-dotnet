# 孙乾（移动版）

> 此版本与 `ol:sun-qian` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-26。独立移动身份 `mobile:sun-qian`，蜀、男性、3体力。

采用[移动版官网武将216](https://www.sanguosha.cn/pc/hero-detail-216.html)的普通身份技能。谦雅在成为锦囊目标后交给另一角色任意张手牌；说盟在出牌阶段结束时与其他有手牌者拼点，赢时由本人对自己使用无中生有，未赢时由对方对本人使用过河拆桥。两条均与当前 `ol:qianya`、`ol:shuomeng` 的完整规则一致，直接引用既有技能 ID，不复制运行代码或再造技能版本。

谦雅沿已有原生锦囊目标窗口、私有手牌选择及真实交牌；说盟沿真实拼点、原始双方与点数结果和原生虚拟普通锦囊返回。共享行为由 `PhaseEndingVirtualTrickChecks` 的既有检查覆盖；本项是内容复用，没有重复人物定义快照。

画像采用官网该普通人物页第一张[原图37600](https://www.sanguosha.cn/storage/uploads/images/skins/37600.jpg)，绑定 `portraitKey=mobile-sun-qian`。素材下载、目录绑定及图鉴整合由本批统一完成。
