# Ecommerce Microservices (.NET 8 + Ocelot + Docker + GitHub Actions)

這是一個最小可行的 ASP.NET Core 微服務範例：
- `user-service`: 回傳使用者資料
- `product-service`: 回傳商品資料
- `gateway`: 使用 Ocelot 作為 API Gateway，統一入口

## 快速開始（本地）
1. 安裝 Docker 與 Docker Compose
2. 在專案根目錄執行：
   ```bash
   docker compose up --build
   ```
3. 測試
   - Gateway: http://localhost:5000/user/1
   - Gateway: http://localhost:5000/product/1
 
## GitHub Actions（CI/CD）
- 建立 GitHub Repo，推上此專案
- 在 **Settings → Secrets and variables → Actions** 建立：
  - `DOCKERHUB_USERNAME`
  - `DOCKERHUB_PASSWORD`
- 推送到 `main` 分支後會：
  - build .NET 專案
  - 以 docker compose 建立三個映像
  - push 到 Docker Hub：
  方法1：
  bash: 
  - docker compose push 
  方法2：
    - `docker push your-dockerhub-user/user-service:latest`
    - `docker push your-dockerhub-user/product-service:latest`
    - `docker push your-dockerhub-user/gateway:latest`

> **記得**：把 `docker-compose.yml` 中的 `your-dockerhub-user` 換成你的 Docker Hub 帳號。


注意事項
先登入 Docker Hub：
輸入你的 Docker Hub 用戶名 和 密碼 
確保映像已建立：在 push 之前，先執行：
GitHub Actions 自動化：根據你的 README，如果你設定好 GitHub Actions 的 secrets，推送到 main 分支就會自動執行 build 和 push 流程。

## 結構
```
ecommerce-microservices/
│── gateway/
│   ├── Gateway.csproj
│   ├── Program.cs
│   ├── ocelot.json
│   └── Dockerfile
│── services/
│   ├── user-service/
│   │   ├── UserService.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/UserController.cs
│   │   └── Dockerfile
│   └── product-service/
│       ├── ProductService.csproj
│       ├── Program.cs
│       ├── Controllers/ProductController.cs
│       └── Dockerfile
│── docker-compose.yml
└── .github/workflows/ci-cd.yml
```

## 常見問題
- **Ocelot 版本問題**：若 restore 發生錯誤，可在 `Gateway.csproj` 中調整 Ocelot 套件版本（例如 17.x/18.x/19.x 皆可），然後重新 build。
- **Windows 埠被占用**：修改 `docker-compose.yml` 的 `ports` 對應埠。
- **要加資料庫？** 可以先在 `docker-compose.yml` 加上 `postgres` 或 `sqlserver`，再在服務內透過 EF Core 連線。
