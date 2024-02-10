import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import './musicDetails.css';
import CardListMusic from '../CardListMusic/CardListMusic';

const MusicDetails = () => {
  let { id } = useParams();
  const [items, setItems] = useState([]);
  const [music, setMusic] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        let url = "https://localhost:7117/Music/GetMusic?" + id;
        const response1 = await fetch(url);
        const data1 = await response1.json();
        setItems(data1);

        let urlList = "https://localhost:7117/Music/GetAll?searchText=" + data1.genre + "&sortField=genre&skip=0";
        console.log(urlList)
        const response2 = await fetch(urlList);
        let data2 = await response2.json();
        console.log(data2.items)
        const filteredMusic = data2.items.filter(item => item.genre === data1.genre).slice(0, 4);
        setMusic(filteredMusic);

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
        <div className="details-image" onClick={() => { getDetails(items.id) }}>
          <img src={items.coverImagePath} alt="img" />
        </div>
        <div className="details-info">
          <h1>{items.name}</h1>
          <div className='details-description'>
            <p>Artist: &nbsp;<span>{items.artist}</span></p>
            <p>Language: <span>{items.language}</span></p>
          </div>
          <div className='d-flex genre'>
            <p>Genre:</p>
            <div className="genreParagraph"><p>{items.genre}</p></div>
          </div>
          {items.hasOwnProperty('rating') ? <Stars rating={items.rating} /> : null}
          <button className="back" onClick={() => navigate('/music', { replace: true })}>Back</button>
        </div>
      </div>
      <div className="more">
        <h2>You can also enjoy:</h2>
        <div className="music">
          {!loading && music.length > 0 && (
            <CardListMusic data={music} />
          )}
          {loading && <p>Loading films...</p>}
        </div>

      </div>
    </div >
  )
}

export default MusicDetails