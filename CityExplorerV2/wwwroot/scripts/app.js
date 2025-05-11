console.log("✅ JS is now loaded and executing");
document.getElementById("search-form").addEventListener("submit", async function (e) {
    e.preventDefault();

    const city = document.getElementById("search-input").value.trim();
    if (!city) return;

    try {
        const [cityResponse, weatherResponse] = await Promise.all([
            fetch(`/api/cityapi/city?name=${encodeURIComponent(city)}`),
            fetch(`/api/cityapi/weather?city=${encodeURIComponent(city)}`)
        ]);

        const cityData = await cityResponse.json();
        const weatherData = await weatherResponse.json();

        displayCityData(cityData, weatherData);
    } catch (error) {
        console.error("Error fetching data:", error);
    }
});

function displayCityData(cityData, weatherData) {
    const cardsContainer = document.querySelector(".cards");
    cardsContainer.innerHTML = "";
    cardsContainer.style.display = "grid";

    if (!cityData.data || cityData.data.length === 0) {
        cardsContainer.innerHTML = "<p>No results found.</p>";
        return;
    }

    cityData.data.forEach(city => {
        const card = document.createElement("article");

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

        cardsContainer.appendChild(card);
    });
}