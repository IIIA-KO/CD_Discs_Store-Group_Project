import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import './filmDetails.css';
import Card from '../../pages/Card/Card';
import CardList from '../CardList/CardList';

const FilmDetails = () => {
  let { id } = useParams();
  const [items, setItems] = useState([]);
  const [films, setFilms] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        let url = "https://localhost:7117/Film/GetFilm?" + id;
        const response1 = await fetch(url);
        const data1 = await response1.json();
        setItems(data1);

        let urlList = "https://localhost:7117/Film/GetAll?searchText=" + data1.genre + "&sortField=genre&skip=0&take=20";
        console.log(urlList)
        const response2 = await fetch(urlList);
        const data2 = await response2.json();
        console.log(data2.items)
        const filteredFilms = data2.items.filter(item => item.genre === data1.genre).slice(0, 4);
        setFilms(filteredFilms);

        setLoading(false);
      } catch (error) {
        console.error(error);
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);
  let navigate = useNavigate();

  return (
    <div>
      <div className="details">
        <div className="details-image">
          <img src={items.coverImagePath} alt="img" />
        </div>
        <div className="details-info">
          <h1>{items.name}</h1>
          <div className='details-description'>
            <p>Producer: &nbsp;<span>{items.producer}</span></p>
            <p>Main role: <span>{items.mainRole}</span></p>
            <p>Age limit: <span>{items.ageLimit}+</span></p>
          </div>
          <div className=' genre'>
            <p>Genre:</p>
            <div className="genreParagraph"><p>{items.genre}</p></div>
          </div>
          {items.hasOwnProperty('rating') ? <Stars rating={items.rating} /> : null}
          <button className="back" onClick={() => navigate('/films', { replace: true })}>Back</button>
        </div>
      </div>
      <div className="more">
        <h2>You can also enjoy:</h2>
        {console.log(films)}
        {console.log(loading)}
        <div className="music">
          {!loading && films.length > 0 && (
            <CardList data={films} />
          )}
          {loading && <p>Loading films...</p>}
        </div>

      </div>
    </div >
  )
}

export default FilmDetails