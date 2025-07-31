# 🧑‍💼 HR Chat Buddy API

A GenAI-powered HR assistant built with .NET Core, OpenAI, Azure, and Pinecone using Retrieval-Augmented Generation (RAG). This API answers employee queries based strictly on internal HR policies.

## 🔧 Tech Stack

- **Backend**: ASP.NET Core Web API (.NET 9)
- **AI Model**: OpenAI GPT (via Azure OpenAI)
- **Vector DB**: Pinecone (for embedding and similarity search)
- **Embedding**: Azure OpenAI Embeddings
- **Storage**: Azure Blob Storage (for HR policy docs)
- **Architecture**: Clean Architecture + RAG-based pipeline
- **Deployment**: Azure App Service

- Ask HR policy-related questions like:
  - “What is the leave policy for new employees?”
  - “How many casual leaves are allowed in a year?”
- RAG-powered: Ensures answers are derived only from approved HR documents.
- Scalable and modular API endpoints.
- Easy to update policies — just upload new PDFs or text files.

