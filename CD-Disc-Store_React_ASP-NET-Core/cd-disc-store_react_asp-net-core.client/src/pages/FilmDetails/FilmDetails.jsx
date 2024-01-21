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

  function mapFilms(){
    return films.map((film) => (
      <Card key={film.id} item={film} />
    ))
  }

  useEffect(() => {
    const fetchData = async () => {
      try {
        let url = "https://localhost:7117/Film/GetFilm?" + id;
        const response1 = await fetch(url);
        const data1 = await response1.json();
        setItems(data1);

        let urlList = "https://localhost:7117/Film/GetAll?skip=0&searchText=" + data1.genre + "&sortOrder=1";
        const response2 = await fetch(urlList);
        const data2 = await response2.json();
        setFilms(data2);

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
        <div className="details-image" >
          <img src="https://w0.peakpx.com/wallpaper/627/792/HD-wallpaper-no-black-blank.jpg" alt="img" />
        </div>
        <div className="details-info">
          <h1>{items.name}</h1>
          <div className='details-description'>
            <p>Producer: &nbsp;<span>{items.producer}</span></p>
            <p>Main role: <span>{items.mainRole}</span></p>
            <p>Age limit: <span>{items.ageLimit}+</span></p>
          </div>
          <div className='d-flex genre'>
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
        <div className="films">
          {!loading && films.length > 0 && (
            mapFilms()
        )}
        {loading && <p>Loading films...</p>}
        </div>
        
      </div>
    </div >
  )
}

export default FilmDetails