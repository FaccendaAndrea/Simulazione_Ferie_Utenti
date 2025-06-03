# Gestione Permessi - Istruzioni di Avvio

## Backend (ASP.NET Core)

### Prerequisiti
- [.NET 7+ SDK](https://dotnet.microsoft.com/download)
- (Opzionale) [SQLite](https://www.sqlite.org/download.html) se vuoi ispezionare il DB

### Avvio locale
1. Apri il terminale nella cartella del progetto.
2. Posizionati nella cartella del backend:
   ```sh
   cd GestionePermessi.Api
   ```
3. Ripristina i pacchetti e applica le migration (il DB viene creato e popolato automaticamente):
   ```sh
   dotnet restore
   dotnet build
   dotnet run
   ```
4. L'API sarà disponibile su `http://localhost:5123` (o la porta configurata).
5. Accedi a Swagger per testare le API: [http://localhost:5123/swagger](http://localhost:5123/swagger)

## Frontend (React)

### Prerequisiti
- [Node.js 18+](https://nodejs.org/)
- [npm](https://www.npmjs.com/) (incluso in Node.js)

### Avvio locale
1. Apri un nuovo terminale nella cartella del progetto.
2. Posizionati nella cartella del frontend:
   ```sh
   cd gestione-permessi-client
   ```
3. Installa le dipendenze:
   ```sh
   npm install
   ```
4. Avvia l'app React:
   ```sh
   npm start
   ```
5. L'app sarà disponibile su [http://localhost:3000](http://localhost:3000)

### Configurazione API URL
- Per ambiente locale, l'API è già configurata su `http://localhost:5123/api`.
- Per il deploy, imposta la variabile d'ambiente `REACT_APP_API_URL` con l'URL pubblico del backend.

---

## Credenziali di test

**Dipendente**
- Email: `giorgiogialli@azienda.com`
- Password: `Password123!`

**Responsabile**
- Email: `csgopro@azienda.com`
- Password: `Password123!`

---

## Note aggiuntive
- Il database viene popolato automaticamente al primo avvio con dati di esempio.
- Puoi testare tutte le funzionalità sia da frontend che da Swagger.
- Per il deploy su cloud (es. Coyeb), segui le istruzioni della piattaforma e assicurati di configurare la porta tramite variabile d'ambiente `PORT` per il backend e la variabile `REACT_APP_API_URL` per il frontend. 