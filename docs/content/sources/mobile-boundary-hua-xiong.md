# 移动版界华雄

核对日期：2026-10-25。

- [移动版官网武将详情120](https://www.sanguosha.cn/index.php/pc/hero-detail-120.html)给出完整的耀武：受到杀伤害时，红色杀令来源选择回复一点体力或摸一张牌，非红色杀令华雄摸一张牌。没有势斩或扬威。
- 独立人物 `boundary:hua-xiong-mobile`，群、男性、六体力，画像键 `boundary-hua-xiong-mobile`，规则集 `sanguosha-mobile`。

耀武与仓库中移动版谋华雄的 `mou-mobile:yaowu` 正文及行为完全相同，因此直接复用该既有技能，不复制程序或声明一个只有名称差异的新节点。新人物只授予耀武，不授予谋华雄的扬威，也不混入 OL 界华雄的势斩及任意牌伤害分支。

沿原生伤害前窗口处理真实杀，按原使用牌冻结的颜色选择受益人。“不为红色”包含黑色及无色；非杀伤害不发动。来源回复或摸牌、本人摸牌及其原生子窗先完成，再继续同一伤害。伤害来源已不存在时沿原程序跳过不能完成的来源收益，不改为其他人物受益。实体牌、虚拟杀、护甲和来源失效后的已发行收益均沿既有原帧及冷恢复合同。

本次为完整纯配置复用，不新增重复夹具。整批沿用 `Mou Hua Xiong Mobile: native Slash colors benefit before Armor HP cold` 与 `Mou Hua Xiong Mobile: paid benefit children source loss native return cold` 检查耀武主路径；统一内容加载及图鉴检查覆盖新增身份与独立立绘。
