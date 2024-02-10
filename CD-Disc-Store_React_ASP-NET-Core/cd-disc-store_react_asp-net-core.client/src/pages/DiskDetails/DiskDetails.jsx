import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import './../FilmDetails/filmDetails.css';
import CardList from '../CardList/CardList';
import Stars from '../../Stars/Stars'
import DiskCardList from '../../DiskCardList/DiskCardList';

const DiskDetails = () => {
  let { id } = useParams();
  const [items, setItems] = useState([]);
  const [films, setFilms] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        let url = "https://localhost:7117/Discs/GetDisc?" + id;
        console.log(url);
        const response1 = await fetch(url);
        console.log(response1)
        const data1 = await response1.json();
        setItems(data1);

        let urlList = "https://localhost:7117/Discs/GetAll?skip=0&take=20";
        console.log(urlList)
        const response2 = await fetch(urlList);
        const data2 = await response2.json();
        data2.items=data2.items.slice(0, 4);
        //console.log(data2.items)
        setFilms(data2.items);

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
            <p>Price: &nbsp;<span>${items.price}</span></p>
            <p>Left on stock: <span>{items.leftOnStock}</span></p>
            <Stars key={items.id} rating={items.rating} />
          </div>
          <button className="back" onClick={() => navigate('/disks', { replace: true })}>Back</button>
        </div>
      </div>
      <div className="more">
        <h2>You can also enjoy:</h2>
        {console.log(films)}
        {console.log(loading)}
        <div className="music">
          {!loading && films.length > 0 && (
            <DiskCardList data={films} />
          )}
          {loading && <p>Loading films...</p>}
        </div>

      </div>
    </div >
  )
}

export default DiskDetails