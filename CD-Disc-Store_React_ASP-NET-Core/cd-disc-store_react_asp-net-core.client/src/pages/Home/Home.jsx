import React from 'react'
import { useEffect, useState } from 'react'
import TopFilms from '../../TopFilms/TopFilms';
import TopMusics from '../../TopMusics/TopMusics';

const Home = () => {
  const [filmItems, setFilmItems] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('https://localhost:7117/Film/GetAll?skip=0');
      const data = await response.json();
      setFilmItems(data.items.slice(0, 10));
    };

    fetchData();
  }, []);

  const [musicItems, setMusicItems] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('https://localhost:7117/Music/GetAll?skip=0');
      const data = await response.json();
      setMusicItems(data.items.slice(0, 10));
    };

    fetchData();
  }, []);

  const [popularfilmItems, setPopularFilmItems] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('https://localhost:7117/Film/GetAll?skip=1');
      const data = await response.json();
      setPopularFilmItems(data.items.slice(0, 10));
    };

    fetchData();
  }, []);

  const [popularmusicItems, setPopularMusicItems] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('https://localhost:7117/Music/GetAll?skip=1');
      const data = await response.json();
      setPopularMusicItems(data.items.slice(0, 10));
    };

    fetchData();
  }, []);

  return (
    <div>
      <h2>Top-10 new film disks</h2>
      <TopFilms data={filmItems} />
      <h2>Top-10 new music disks</h2>
      <TopMusics data={musicItems} />
      <h2>Top-10 popular film disks</h2>
      <TopFilms data={popularfilmItems} />
      <h2>Top-10 popular music disks</h2>
      <TopMusics data={popularmusicItems} />
    </div>
  );
};

export default Home;