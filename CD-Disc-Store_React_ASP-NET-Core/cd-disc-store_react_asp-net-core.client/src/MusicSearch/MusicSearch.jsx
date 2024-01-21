import React, { useState, useEffect } from 'react';
import "./MusicSearch.css"
import CardListMusic from '../pages/CardListMusic/CardListMusic';
const MusicSearch = ({ musics }) => {
    const [search, setSearch] = useState('');
  const [genreFilter, setGenreFilter] = useState('');
  const [filteredMusics, setFilteredMuiscs] = useState([]);

  useEffect(() => {
    const filtered = musics.filter((music) => {
      const titleMatch = music.name.toLowerCase().includes(search.toLowerCase());
      const genreMatch = music.genre.toLowerCase().includes(genreFilter.toLowerCase());
      return titleMatch && genreMatch;
    });
    setFilteredMuiscs(filtered);
  }, [search, genreFilter, musics]);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);
  }

  const handleGenreChange = (e) => {
    setGenreFilter(e.target.value);
  };


  return (
    <div>
        <div className='search_bar'>
      <input type="text" placeholder='search musics' value={search} onChange={handleSearchChange} />
      <select value={genreFilter} onChange={handleGenreChange}>
        <option value="">All genres</option>
        <option value="world">World</option>
        <option value="metal">Metal</option>
        <option value="jazz">Jazz</option>
        <option value="rock">Rock</option>
        <option value="Blues">Blues</option>
        <option value="nev_age">Nev Age</option>
        <option value="r&b">R&B</option>
        <option value="country">Country</option>
        <option value="foreign">Foreign</option>
        <option value="romance">Romance</option>
        {/* Другие жанры */}
      </select>
      </div>
      {/* Отображение отфильтрованных результатов */}
      <ul>
        {/*{filteredMovies.map(movie => (
          <li key={movie.id}>{movie.name} - {movie.genre}</li>
        ))}*/}
        <CardListMusic data={filteredMusics} />
      </ul>
    </div>
  );
};

export default MusicSearch;
