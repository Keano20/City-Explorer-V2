// Switch between cities and users tabs based on the selected tab
function showTab(tab) {
    document.getElementById("cities-tab").style.display = tab === "cities" ? "block" : "none";
    document.getElementById("users-tab").style.display = tab === "users" ? "block" : "none";
}

// Fetch and display all cities
async function fetchCities() {
    const res = await fetch('/api/city');
    const data = await res.json();
    const container = document.getElementById('cities-container');
    container.innerHTML = ''; // Clear any existing content

    data.forEach(city => {
        // Creates the HTML structure for each city
        const cityHTML = `
        <article id="city-${city.id}" class="city-entry">
            <div class="view-mode">
                <p><strong>${city.name}</strong> (${city.country} – ${city.region})</p>
                <button onclick="startEditCity('${city.id}', '${city.name}', '${city.country}', '${city.region}')">Edit</button>
                <button onclick="deleteCity('${city.id}')">Delete</button>
            </div>
            <div class="edit-mode" style="display: none;">
                <input type="text" id="edit-name-${city.id}" value="${city.name}" />
                <input type="text" id="edit-country-${city.id}" value="${city.country}" />
                <input type="text" id="edit-region-${city.id}" value="${city.region}" />
                <button onclick="submitEditCity('${city.id}')">Save</button>
                <button onclick="cancelEditCity('${city.id}')">Cancel</button>
            </div>
        </article>
        `;

        container.insertAdjacentHTML('beforeend', cityHTML); // Add to DOM
    });
}

// Sends a Delete request for a city and then refreshes the list
async function deleteCity(id) {
    await fetch(`/api/city/${id}`, { method: 'DELETE' });
    fetchCities();
}

// Shows the editable fields for a selected city
function startEditCity(id, name, country, region) {
    const article = document.getElementById(`city-${id}`);
    article.querySelector('.view-mode').style.display = 'none';
    article.querySelector('.edit-mode').style.display = 'block';
}

// Cancels editing and shows the original view
function cancelEditCity(id) {
    const article = document.getElementById(`city-${id}`);
    article.querySelector('.edit-mode').style.display = 'none';
    article.querySelector('.view-mode').style.display = 'block';
}

// Send a Put request with updated city info the refresh the list
async function submitEditCity(id) {
    const updatedCity = {
        name: document.getElementById(`edit-name-${id}`).value.trim(),
        country: document.getElementById(`edit-country-${id}`).value.trim(),
        region: document.getElementById(`edit-region-${id}`).value.trim()
    };

    await fetch(`/api/city/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedCity)
    });

    fetchCities(); // Refreshes the list
}

// Handles submission of the Add City form
document.getElementById('add-city-form').addEventListener('submit', async (event) => {
    event.preventDefault();

    const newCity = {
        name: document.getElementById('city-name').value,
        country: document.getElementById('city-country').value,
        region: document.getElementById('city-region').value
    };

    await fetch('/api/city', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newCity)
    });
    
    event.target.reset(); // Clear form
    fetchCities(); // Refresh city list
});

// Fetch and display all users
async function fetchUsers() {
    const res = await fetch('/api/user');
    const data = await res.json();
    const container = document.getElementById('users-container');
    container.innerHTML = data.map(user => `
        <article>
            <p><strong>${user.username}</strong></p>
            <button onclick="deleteUser('${user.id}')">Delete</button>
        </article>
    `).join('');
}

// Send a delete request for a user and refresh the list
async function deleteUser(id) {
    await fetch(`/api/user/${id}`, { method: 'DELETE' });
    fetchUsers();
}

// Initial page load: fetch cities and users
fetchCities();
fetchUsers();