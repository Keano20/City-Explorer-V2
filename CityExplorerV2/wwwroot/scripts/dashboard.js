function showTab(tab) {
    document.getElementById("cities-tab").style.display = tab === "cities" ? "block" : "none";
    document.getElementById("users-tab").style.display = tab === "users" ? "block" : "none";
}

async function fetchCities() {
    const res = await fetch('/api/city');
    const data = await res.json();
    const container = document.getElementById('cities-container');
    container.innerHTML = data.map(city => `
        <article>
            <p><strong>${city.name}</strong> (${city.country} – ${city.region})</p>
            <button onclick="deleteCity('${city.id}')">Delete</button>
        </article>
    `).join('');
}

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

async function deleteCity(id) {
    await fetch(`/api/city/${id}`, { method: 'DELETE' });
    fetchCities();
}

async function deleteUser(id) {
    await fetch(`/api/user/${id}`, { method: 'DELETE' });
    fetchUsers();
}

fetchCities();
fetchUsers();