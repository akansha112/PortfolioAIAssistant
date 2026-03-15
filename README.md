# 🚀 Portfolio AI Assistant
### *Your Digital Twin: Powered by RAG, .NET 8, and React*

<p align="center">
  <img src="https://github.com/user-attachments/assets/ba3e9d38-d1b4-4a16-9cbe-c40e03095667" alt="Portfolio AI Assistant Hero" width="800px" style="border-radius: 15px; border: 1px solid #444;">
</p>

---

## 🌟 Overview

**Portfolio AI Assistant** is a sophisticated full-stack application that represents my professional profile through an interactive AI interface. Unlike standard chatbots, this system utilizes **RAG (Retrieval-Augmented Generation)**, meaning it generates answers based specifically on my actual resume data stored in a vector database.

---

## 🚀 Key Features

- **🧠 RAG Intelligence**: Advanced retrieval using Pinecone Vector DB and Google Gemini Embeddings for high accuracy.
- **🔄 Smart Context Refresh**: Automatically clears old vector data when a new resume is uploaded, ensuring the AI always stays updated.
- **💎 Glassmorphism UI**: A modern, futuristic aesthetic using backdrop filters, transparency, and sleek animations.
- **⚡ High Performance**: Optimized .NET backend providing millisecond response times for vector queries.
- **📱 Fully Responsive**: Seamless experience across Desktop, Tablet, and Mobile devices.

---

## 🛠️ Tech Stack

<p align="left">
  <img src="https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB" />
  <img src="https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white" />
  <img src="https://img.shields.io/badge/Pinecone-000000?style=for-the-badge&logo=pinecone&logoColor=white" />
  <img src="https://img.shields.io/badge/Google%20Gemini-4285F4?style=for-the-badge&logo=googlegemini&logoColor=white" />
  <img src="https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white" />
</p>

---

## 📸 Screenshots

<p align="center">
  <img alt="Image" src="https://github.com/user-attachments/assets/ba3e9d38-d1b4-4a16-9cbe-c40e03095667" width="45%" alt="Chat Interface" style="margin-right: 10px; border-radius: 10px;" />

<img alt="Image" src="https://github.com/user-attachments/assets/7653da82-47ef-44db-9337-084c8b9ca421" width="45%" alt="Admin Panel" style="border-radius: 10px;" />
 </p>

---

## 🏗️ System Architecture (RAG Pipeline)

1. **Upload**: Admin uploads a PDF or Text resume via the secure dashboard.
2. **Cleanup**: The system triggers a namespace delete in Pinecone to wipe old data.
3. **Embedding**: Gemini API converts text chunks into 768-dimension vectors.
4. **Retrieval**: When a user asks a question, the system finds the top 5 most relevant context chunks.
5. **Generation**: The AI processes the query alongside the retrieved context to deliver a professional response.

---

## ⚙️ Installation & Setup

### 1. Clone the repository
```bash
git clone [https://github.com/akanshasaxena/PortfolioAIAssistant.git](https://github.com/akanshasaxena/PortfolioAIAssistant.git)
