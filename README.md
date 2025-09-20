# Bing Social Media Pulse Hack

A web application that integrates with Bing's Grounding Search API to provide search functionality with a modern UI.

## Features

- Static file serving with a modern dashboard interface
- Integration with Bing Grounding Search API
- Real-time search functionality
- Interactive suggestion buttons
- Loading states and error handling
- Modern UI with smooth animations

## Setup

1. Configure the Bing API settings:
   - Open `appsettings.json`
   - Update the `BingApiSettings:AppId` with your Bing API App ID
   - The endpoint is pre-configured

2. Run the application:
   ```bash
   dotnet run
   ```

3. Access the application:
   - Open your browser
   - Navigate to https://localhost:7050
   - The Dashboard.html will load by default

## Usage

1. Enter a search query:
   - Type directly in the search box at the bottom
   - Click any suggestion button to populate a query
   - Press Enter or click the send button to search

2. View Results:
   - Search results are logged to the browser console (F12)
   - Loading states indicate search progress
   - Error messages display if something goes wrong

## Technical Details

- Built with .NET 9.0
- Uses MVC pattern with API controllers
- Static file middleware for serving web content
- Secure configuration management
- Responsive modern UI design

## Development

To extend the application:
1. Add new API endpoints in SearchController.cs
2. Enhance the UI in Dashboard.html
3. Add configuration options in appsettings.json
