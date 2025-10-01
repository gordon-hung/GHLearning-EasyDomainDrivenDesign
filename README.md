# GHLearning-EasyDomainDrivenDesign

[![.NET](https://github.com/gordon-hung/GHLearning-EasyDomainDrivenDesign/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gordon-hung/GHLearning-EasyDomainDrivenDesign/actions/workflows/dotnet.yml)

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/gordon-hung/GHLearning-EasyDomainDrivenDesign)

[![codecov](https://codecov.io/github/gordon-hung/GHLearning-EasyDomainDrivenDesign/graph/badge.svg?token=QyVN42dffl)](https://codecov.io/github/gordon-hung/GHLearning-EasyDomainDrivenDesign)

## 專案簡介

本專案以「公告管理系統」為例，實作了 CRUD 與完整生命週期（草稿 → 發布 → 歸檔）。重點在於展示 **DDD 與 Clean Architecture** 如何協助建立可維護、可擴充的後端系統架構。

## 核心設計

* **Clean Architecture 分層**：Domain、Application、Infrastructure、Web API
* **DDD 元件**：實體 (Entity)、值物件 (Value Object)、Repository、聚合根、領域事件
* **CQRS 模式**：以 MediatR 分離 Command / Query
* **事件驅動**：透過 RabbitMQ 發佈與處理領域事件
* **資料持久化**：使用 MongoDB 實作 Repository

## 技術棧


| 元件      | 技術                |
| --------- | ------------------- |
| 平台      | .NET 9.0            |
| 命令/查詢 | MediatR             |
| 資料庫    | MongoDB             |
| 訊息佇列  | RabbitMQ            |
| 測試      | xUnit + NSubstitute |

## 專案結構

```
📁 Domain          // 核心領域模型、值物件、領域事件
📁 Application     // Commands、Queries、應用邏輯
📁 Infrastructure  // MongoDB、外部整合
📁 WebApi          // RESTful API
📁 Tests           // 單元與領域測試
```

## 快速開始

1. 安裝 **.NET 9.0**、**MongoDB**、**RabbitMQ**
2. Clone 專案並設定 `appsettings.json`
3. 執行 **WebApi** 專案，使用 Swagger 或 Postman 測試端點

## 學習重點

* 建立以 **領域為中心** 的業務邏輯
* 運用 **CQRS + 事件驅動** 提升系統擴展性
* 撰寫可測試、易維護的架構

## 授權

本專案採用 MIT 授權，詳見 [LICENSE](https://chatgpt.com/c/LICENSE)。
