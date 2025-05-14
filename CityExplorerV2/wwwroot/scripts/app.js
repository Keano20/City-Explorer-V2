// Log confirmation that the JS has loaded (was used for debugging)
console.log("✅ JS is now loaded and executing");

// Add submit event listener to the research form
document.getElementById("search-form").addEventListener("submit", async function (event) {
    event.preventDefault(); // Prevents the page from reloading

    const city = document.getElementById("search-input").value.trim();
    if (!city) return; // Exit if input is empty

    try {
        // Fetch city and weather data server-side API
        const [cityResponse, weatherResponse] = await Promise.all([
            fetch(`/api/cityapi/city?name=${encodeURIComponent(city)}`),
            fetch(`/api/cityapi/weather?city=${encodeURIComponent(city)}`)
        ]);

        const cityData = await cityResponse.json();
        const weatherData = await weatherResponse.json();
        
        // Display the results
        displayCityData(cityData, weatherData);
    } catch (error) {
        console.error("Error fetching data:", error);
    }
});

// Populate the page with the city and weather data
function displayCityData(cityData, weatherData) {
    const cardsContainer = document.querySelector(".cards");
    cardsContainer.innerHTML = ""; // Clear the previous results
    cardsContainer.style.display = "grid"; 

    // If no city results are found
    if (!cityData.data || cityData.data.length === 0) {
        cardsContainer.innerHTML = "<p>No results found.</p>";
        return;
    }
    
    // Loop though each city and create a card
    cityData.data.forEach(city => {
        const card = document.createElement("article");
        
        // Creates the cards HTML with city and weather info
        card.innerHTML = `
            <h2>${city.city}</h2>
            <p><strong>Region:</strong> ${city.region}</p>
            <p><strong>Country:</strong> ${city.country}</p>
            <hr>
            <h3>Current Weather</h3>
            <p><strong>Temp:</strong> ${weatherData.current.temp_c} °C / ${weatherData.current.temp_f} °F</p>
            <p><strong>Feels Like:</strong> ${weatherData.current.feelslike_c} °C / ${weatherData.current.feelslike_f} °F</p>
            <p><strong>Humidity:</strong> ${weatherData.current.humidity}%</p>
        `;
        
        // Add the card to the container
        cardsContainer.appendChild(card);
    });
}